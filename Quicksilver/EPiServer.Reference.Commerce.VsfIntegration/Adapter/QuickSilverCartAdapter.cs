using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.VsfIntegration.Service;
using EPiServer.Vsf.ApiBridge.Utils;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model.Cart;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using EPiServer.Vsf.Core.Exporting;
using EPiServer.Vsf.DataExport.Utils.Epi;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using PaymentMethod = EPiServer.Vsf.Core.ApiBridge.Model.Cart.PaymentMethod;

namespace EPiServer.Reference.Commerce.VsfIntegration.Adapter
{
    public class QuickSilverCartAdapter : ICartAdapter
    {
        private readonly IContentLoaderWrapper _contentLoaderWrapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IPromotionEngine _promotionEngine;
        private readonly IVsfPriceService _priceService;
        private readonly IMarketService _marketService;
        private readonly IEnumerable<IPaymentMethod> _paymentMethods;
        private readonly ShippingManagerFacade _shippingManagerFacade;
        private readonly ReferenceConverter _referenceConverter;


        public QuickSilverCartAdapter(IContentLoaderWrapper contentLoaderWrapper, 
            IOrderRepository orderRepository, 
            IPromotionEngine promotionEngine, 
            IVsfPriceService priceService,
            IMarketService marketService,
            IEnumerable<IPaymentMethod> paymentMethods,
            ShippingManagerFacade shippingManagerFacade,
            ReferenceConverter referenceConverter)
        {
            _contentLoaderWrapper = contentLoaderWrapper;
            _orderRepository = orderRepository;
            _promotionEngine = promotionEngine;
            _priceService = priceService;
            _marketService = marketService;
            _paymentMethods = paymentMethods;
            _shippingManagerFacade = shippingManagerFacade;
            _referenceConverter = referenceConverter;
        }

        public string DefaultCartName => "vsf-default-cart";

        public string CreateCart(Guid contactId)
        {
            using (CartLocker.Lock(contactId))
            {
                var cart = _orderRepository.LoadOrCreateCart<ICart>(contactId, DefaultCartName);
                _orderRepository.Save(cart);
                return cart.CustomerId.ToString();
            }
        }

        public IEnumerable<CartItem> Pull(Guid contactId)
        {
            using (CartLocker.Lock(contactId))
            {
                var cart = GetCart(contactId);
                var cartItems = cart?.GetAllLineItems();
                return cartItems?.Select(item => CreateCartItem(item, contactId.ToString()));
            }
        }

        public CartItem Update(Guid contactId, CartItem cartItem)
        {
            using (CartLocker.Lock(contactId))
            {
                var cart = GetCart(contactId);

                if (cart == null || cartItem == null)
                    return null;

                var updatedItem = cart.GetAllLineItems().FirstOrDefault(item => item.Code == cartItem.Sku);

                if (updatedItem != null)
                {
                    var shipment = cart.GetFirstShipment();
                    cart.UpdateLineItemQuantity(shipment, updatedItem, cartItem.Qty);
                }
                else
                {
                    updatedItem = cart.CreateLineItem(cartItem.Sku);
                    updatedItem.Quantity = cartItem.Qty;
                    UpdateCartLine(updatedItem);
                    cart.AddLineItem(updatedItem);
                }

                _orderRepository.Save(cart);
                return CreateCartItem(updatedItem, contactId.ToString());
            }
        }

        public bool Delete(Guid contactId, CartItem cartItem)
        {
            using (CartLocker.Lock(contactId))
            {
                var cart = GetCart(contactId);
                var itemToDelete = cart.GetAllLineItems().FirstOrDefault(item => item.LineItemId == cartItem.ItemId);
                if (itemToDelete != null)
                {
                    var shipment = cart.GetFirstShipment();
                    var result = shipment.LineItems.Remove(itemToDelete);
                    shipment.LineItems.Remove(itemToDelete);
                    _orderRepository.Save(cart);
                    return result;
                }
                return false;
            }
        }

        public Total GetTotals(Guid contactId)
        {
            var cart = GetCart(contactId);
            return CreateTotal(cart);
        }

        public IEnumerable<PaymentMethod> GetPaymentMethods(Guid contactId)
        {
            var cart = GetCart(contactId);
            var market = _marketService.GetMarket(cart.MarketId);

            return PaymentManager.GetPaymentMethodsByMarket(market.MarketId.Value)
                .PaymentMethod
                .Where(x => x.IsActive && string.Equals(market.DefaultLanguage.TwoLetterISOLanguageName, x.LanguageId,
                                StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Ordering)
                .Select(x =>
                {
                    var pm = _paymentMethods.SingleOrDefault(method => method.SystemKeyword == x.SystemKeyword);
                    return new PaymentMethod()
                    {
                        Title = pm.Name,
                        Code = pm.PaymentMethodId.ToString()
                    };
                });
        }

        public IEnumerable<ShippingMethod> GetShippingMethods(Guid contactId, UserAddressModel address)
        {
            var cart = GetCart(contactId);
            return cart.GetFirstForm().Shipments.SelectMany(shipment => CreateShippingMethod(cart.MarketId, cart.Currency, shipment));
        }

        public bool AddCoupon(Guid contactId, string couponCode)
        {
            var cart = GetCart(contactId);

            var couponCodes = cart.GetFirstForm().CouponCodes;
            if (couponCodes.Any())
            {
                //Vue Storefront allows to apply only one coupon code
                couponCodes.Clear();
            }
            couponCodes.Add(couponCode);
            var rewardDescriptions = ApplyDiscounts(cart);
            var appliedCoupons = rewardDescriptions
                .Where(r => r.AppliedCoupon != null)
                .Select(r => r.AppliedCoupon);

            var couponApplied = appliedCoupons.Any(c => c.Equals(couponCode, StringComparison.OrdinalIgnoreCase));
            if (!couponApplied)
            {
                couponCodes.Remove(couponCode);
            }
            return couponApplied;
        }

        public string GetCartCoupon(Guid contactId)
        {
            var cart = GetCart(contactId);
            return cart.GetFirstForm().CouponCodes.FirstOrDefault();
        }

        public bool DeleteCoupon(Guid contactId)
        {
            var cart = GetCart(contactId);
            var couponCodes = cart.GetFirstForm().CouponCodes;
            if (couponCodes.Any())
            {
                couponCodes.Clear();
                ApplyDiscounts(cart);
                return true;
            }

            return false;
        }

        private IEnumerable<RewardDescription> ApplyDiscounts(ICart cart)
        {
            return cart.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());
        }

        private ICart GetCart(Guid contactId)
        {
            return _orderRepository.Load<ICart>(contactId, DefaultCartName).FirstOrDefault();
        }

        private CartItem CreateCartItem(ILineItem item, string cartId)
        {
            var cartItem = new CartItem
            {
                ItemId = item.LineItemId,
                Sku = item.Code,
                Qty = (int) item.Quantity,
                Name = item.DisplayName,
                Price = (int) item.PlacedPrice,
                ProductType = "configurable",
                QuoteId = cartId
            };

            return cartItem;
        }

        private void UpdateCartLine(ILineItem updatedItem)
        {

            
            var variationLinkt = _referenceConverter.GetContentLink(updatedItem.Code);
            var variation = _contentLoaderWrapper.Get<VariationContent>(variationLinkt);

            if (variation != null)
            {
                updatedItem.DisplayName = variation.DisplayName;
                updatedItem.PlacedPrice = _priceService.GetDefaultPrice(variation.PriceReference);
            }
        }

        private IEnumerable<ShippingMethod> CreateShippingMethod(MarketId marketId, Currency currency, IShipment shipment)
        {
            var market = _marketService.GetMarket(marketId);
            var shippingRates = GetShippingRates(market, currency, shipment);
            return shippingRates.Any()
                ? shippingRates.Select(r => new ShippingMethod { CarrierCode = shipment.ShippingMethodId.ToString(), MethodCode = r.Id.ToString(), MethodTitle = r.Name, Amount = r.Money.Amount, Available = true})
                : Enumerable.Empty<ShippingMethod>();
        }

        private IEnumerable<ShippingRate> GetShippingRates(IMarket market, Currency currency, IShipment shipment)
        {
            var methods = _shippingManagerFacade.GetShippingMethodsByMarket(market.MarketId.Value, false);
            var currentLanguage = market.DefaultLanguage.TwoLetterISOLanguageName;

            return methods.Where(shippingMethodRow => string.Equals(currentLanguage, shippingMethodRow.LanguageId, StringComparison.OrdinalIgnoreCase)
                                                      && string.Equals(currency, shippingMethodRow.Currency, StringComparison.OrdinalIgnoreCase))
                .OrderBy(shippingMethodRow => shippingMethodRow.Ordering)
                .Select(shippingMethodRow => _shippingManagerFacade.GetRate(shipment, shippingMethodRow, market))
                .Where(rate => rate != null);
        }


        private static Total CreateTotal(ICart cart)
        {
            var items = cart.GetAllLineItems().ToList();

            return new Total
            {
                GrandTotal = cart.GetTotal().Amount,
                WeeeTaxAppliedAmount = null,
                BaseCurrencyCode = cart.Currency.CurrencyCode,
                QuoteCurrencyCode = cart.Currency.CurrencyCode,
                ItemsQty = items.Count(),
                Items = items.Select(CreateTotalItem).ToList(),
                TotalSegments = CreateSegments(cart)
            };
        }

        private static List<TotalSegment> CreateSegments(ICart cart)
        {
            var result = new List<TotalSegment>();
            var subTotal = cart.GetSubTotal();
            result.Add(new TotalSegment
            {
                Code = "subtotal",
                Title = "Subtotal",
                Value = (long?)subTotal.Amount
            });

            var shippingTotal = cart.GetShippingTotal();
            result.Add(new TotalSegment
            {
                Code = "shipping",
                Title = "Shipping",
                Value = (long?)shippingTotal.Amount
            });
            result.Add(new TotalSegment
            {
                Code = "handling",
                Title = "Handling",
                Value = (long?)shippingTotal.Amount
            });
            var grandTotal = cart.GetTotal();
            result.Add(new TotalSegment
            {
                Code = "grand_total",
                Title = "Grant Total",
                Value = (long?)grandTotal.Amount
            });
            return result;
        }

        private static TotalItem CreateTotalItem(ILineItem item)
        {
            //TODO this may not be fully implemented
            
            return new TotalItem
            {
                ItemId = item.LineItemId,
                Price = (long)item.PlacedPrice,
                BasePrice = (long)item.PlacedPrice,
                Qty = (long)item.Quantity,
                RowTotal = 0,
                BaseRowTotal = 0,
                RowTotalWithDiscount = 0,
                TaxAmount = 0,
                BaseTaxAmount = 0,
                TaxPercent = 0,
                DiscountAmount = 0,
                BaseDiscountAmount = 0,
                DiscountPercent = 0,
                Options = "",
                WeeeTaxAppliedAmount = null,
                WeeeTaxApplied = null,
                Name = item.DisplayName,
                ProductOption = new ProductOption()
            };
        }
    }
}
