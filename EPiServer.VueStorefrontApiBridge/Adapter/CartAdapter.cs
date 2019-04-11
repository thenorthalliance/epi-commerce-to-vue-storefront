using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.Core.ApiBridge;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Cart;
using EPiServer.Vsf.DataExport.Utils.Epi;
using Mediachase.Commerce.Catalog;

namespace EPiServer.VueStorefrontApiBridge.Adapter
{
    public class CartAdapter : ICartAdapter
    {
        private readonly IOrderRepository _orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>();
        private readonly IPromotionEngine _promotionEngine = ServiceLocator.Current.GetInstance<IPromotionEngine>();

        public string DefaultCartName => "vsf-default-cart";

        public string CreateCart(Guid contactId)
        {
            var cart = _orderRepository.LoadOrCreateCart<ICart>(contactId, DefaultCartName);
            _orderRepository.Save(cart); //TODO since CreateCart and Update methods are called simultaneously, this may introduce inconsistency
            return cart.CustomerId.ToString();
        }

        public IEnumerable<CartItem> Pull(Guid contactId)
        {
            var cart = GetCart(contactId);
            var cartItems = cart?.GetAllLineItems();
            return cartItems?.Select(item => CreateCartItem(item, contactId.ToString()));
        }

        public CartItem Update(Guid contactId, CartItem cartItem)
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

            _orderRepository.Save(cart); //TODO since CreateCart and Update methods are called simultaneously, this may introduce inconsistency
            return CreateCartItem(updatedItem, contactId.ToString());
        }

        public bool Delete(Guid contactId, CartItem cartItem)
        {
            var cart = GetCart(contactId);
            var itemToDelete = cart.GetAllLineItems().FirstOrDefault(item => item.LineItemId == cartItem.ItemId);
            if (itemToDelete != null)
            {
                var shipment = cart.GetFirstShipment();
                var result = shipment.LineItems.Remove(itemToDelete);
                shipment.LineItems.Remove(itemToDelete);
                _orderRepository.Save(cart); //TODO since CreateCart and Update methods are called simultaneously, this may introduce inconsistency
                return result;
            }
            return false;
        }

        public Total GetTotals(Guid contactId)
        {
            var cart = GetCart(contactId);
            return CreateTotal(cart);
        }

        public IEnumerable<PaymentMethod> GetPaymentMethods(Guid contactId)
        {
            var cart = GetCart(contactId);
            return cart.GetFirstForm().Payments.Select(payment =>
                new PaymentMethod {Code = payment.PaymentMethodName.Replace(" ", "").ToLower(), Title = payment.PaymentMethodName});
        }

        public IEnumerable<ShippingMethod> GetShippingMethods(Guid contactId, UserAddressModel address)
        {
            var cart = GetCart(contactId);
            return cart.GetFirstForm().Shipments.Select(CreateShippingMethod);
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
            var referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var priceService = ServiceLocator.Current.GetInstance<PriceService>();
            
            var variationLinkt = referenceConverter.GetContentLink(updatedItem.Code);
            var variation = contentLoader?.Get<VariationContent>(variationLinkt);
            if (variation != null)
            {
                updatedItem.DisplayName = variation.DisplayName;
                updatedItem.PlacedPrice = priceService.GetDefaultPrice(updatedItem.Code);
            }
        }

        private static ShippingMethod CreateShippingMethod(IShipment shipment)
        {
            return new ShippingMethod
            {
                MethodCode = shipment.ShippingMethodName?.Replace(" ", "").ToLower(),
                MethodTitle = shipment.ShippingMethodName,
                Available = shipment.CanBePacked()
            };
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
            //TODO get products options
            
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
