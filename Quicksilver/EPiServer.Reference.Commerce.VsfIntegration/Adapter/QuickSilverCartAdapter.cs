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
        private readonly OrderValidationService _orderValidationService;

        public QuickSilverCartAdapter(IContentLoaderWrapper contentLoaderWrapper, 
            IOrderRepository orderRepository, 
            IPromotionEngine promotionEngine, 
            IVsfPriceService priceService,
            IMarketService marketService,
            IEnumerable<IPaymentMethod> paymentMethods,
            ShippingManagerFacade shippingManagerFacade,
            ReferenceConverter referenceConverter,
            OrderValidationService orderValidationService)
        {
            _contentLoaderWrapper = contentLoaderWrapper;
            _orderRepository = orderRepository;
            _promotionEngine = promotionEngine;
            _priceService = priceService;
            _marketService = marketService;
            _paymentMethods = paymentMethods;
            _shippingManagerFacade = shippingManagerFacade;
            _referenceConverter = referenceConverter;
            _orderValidationService = orderValidationService;
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
            return cart.GetFirstForm().Shipments.SelectMany(shipment => GetShippingMethods(cart.MarketId, cart.Currency, shipment));
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

        public ICart GetCart(Guid contactId)
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

        private IEnumerable<ShippingMethod> GetShippingMethods(MarketId marketId, Currency currency, IShipment shipment)
        {
            var market = _marketService.GetMarket(marketId);
            var shippingRates = GetShippingRates(market, currency, shipment).ToList();
            return shippingRates.Any()
                ? shippingRates.Select(r => new ShippingMethod { CarrierCode = shipment.ShipmentId.ToString(), MethodCode = r.Id.ToString(), MethodTitle = r.Name, Amount = r.Money.Amount, Available = true})
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

        public void UpdateShippingMethod(Guid contactId, int shipmentId, Guid shippingMethodId)
        {
            var cart = GetCart(contactId);
            var shipment = cart.GetFirstForm().Shipments.First(x => x.ShipmentId == shipmentId);
            shipment.ShippingMethodId = shippingMethodId;
            ValidateCart(cart);
            _orderRepository.Save(cart);
        }

        public IDictionary<ILineItem, IList<ValidationIssue>> ValidateCart(ICart cart)
        {
            return _orderValidationService.ValidateOrder(cart);
        }

        private Total CreateTotal(ICart cart)
        {
            var items = cart.GetAllLineItems().ToList();

            return new Total
            {
                GrandTotal = cart.GetTotal().Amount,
                WeeeTaxAppliedAmount = null,
                BaseCurrencyCode = cart.Currency.CurrencyCode,
                QuoteCurrencyCode = cart.Currency.CurrencyCode,
                ItemsQty = items.Count(),
                Items = items.Select(item => CreateTotalItem(item, cart)).ToList(),
                TotalSegments = CreateSegments(cart)
            };
        }

        private List<TotalSegment> CreateSegments(ICart cart)
        {
            var result = new List<TotalSegment>();
            var totals = cart.GetOrderGroupTotals();
            
            result.Add(new TotalSegment
            {
                Code = "subtotal",
                Title = "Subtotal",
                Value = totals.SubTotal.Amount
            });

            if (totals.TaxTotal.Amount > 0)
            {
                result.Add(new TotalSegment
                {
                    Code = "tax",
                    Title = "Tax",
                    Value = totals.TaxTotal.Amount
                });
            }

            if (totals.ShippingTotal.Amount > 0)
            {
                result.Add(new TotalSegment
                {
                    Code = "shipping",
                    Title = "Shipping Cost",
                    Value = totals.ShippingTotal.Amount
                });
            }

            var shippingDiscount = cart.GetShippingDiscountTotal();

            if (shippingDiscount.Amount > 0)
            {
                result.Add(new TotalSegment
                {
                    Code = "shippingDiscount",
                    Title = "Shipping Discount",
                    Value = -shippingDiscount.Amount
                });
            }

            var orderDiscount = cart.GetOrderDiscountTotal();

            if (orderDiscount.Amount > 0)
            {
                result.Add(new TotalSegment
                {
                    Code = "orderDiscount",
                    Title = "Order Discount",
                    Value = -orderDiscount.Amount
                });
            }

            result.Add(new TotalSegment
            {
                Code = "grand_total",
                Title = "Grand Total",
                Value = totals.Total.Amount
            });

            return result;
        }

        private TotalItem CreateTotalItem(ILineItem item, ICart cart)
        {
            var lineItemPrices = item.GetLineItemPrices(cart.Currency);

            //TODO: implement taxes, GetSalesTax??
            var totalItem = new TotalItem
            {
                ItemId = item.LineItemId,
                Price = item.PlacedPrice,
                BasePrice = item.PlacedPrice,
                Qty = item.Quantity,
                RowTotal = item.Quantity * item.PlacedPrice,
                BaseRowTotal = item.Quantity * item.PlacedPrice,
                RowTotalWithDiscount = lineItemPrices.DiscountedPrice.Amount,
                TaxAmount = 0, //TODO: taxes, GetSalesTax??
                BaseTaxAmount = 0, //TODO: taxes, GetSalesTax??
                TaxPercent = 0, //TODO: taxes, GetSalesTax??
                DiscountAmount = item.GetDiscountTotal(cart.Currency).Amount,
                BaseDiscountAmount = item.GetDiscountTotal(cart.Currency).Amount,
                DiscountPercent = 0,
                PriceIncludingTax = 0, //TODO: taxes, GetSalesTax??
                BasePriceIncludingTax = 0, //TODO: taxes, GetSalesTax??
                RowTotalIncludingTax = item.Quantity * item.PlacedPrice, //TODO: taxes, GetSalesTax??
                BaseRowTotalIncludingTax = item.Quantity * item.PlacedPrice, //TODO: taxes, GetSalesTax??
                Options = "", //TODO: options like colors etc?
                WeeeTaxAppliedAmount = null,
                WeeeTaxApplied = null,
                Name = item.DisplayName,
                ProductOption = new ProductOption()
            };

            return totalItem;
        }
    }
}
