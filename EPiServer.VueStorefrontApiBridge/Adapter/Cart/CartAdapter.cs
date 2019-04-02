using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.ApiModel.Cart;
using PaymentMethod = EPiServer.VueStorefrontApiBridge.ApiModel.Cart.PaymentMethod;

namespace EPiServer.VueStorefrontApiBridge.Adapter.Cart
{
    public class CartAdapter : ICartAdapter
    {
        private readonly IOrderRepository _orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>();
        private readonly IPromotionEngine _promotionEngine = ServiceLocator.Current.GetInstance<IPromotionEngine>();

        public string DefaultCartName => "Default";

        public string CreateCart(Guid contactId)
        {
            var cart = _orderRepository.LoadOrCreateCart<ICart>(contactId, DefaultCartName);
            _orderRepository.Save(cart);
            return cart.CustomerId.ToString();
        }

        public IEnumerable<CartItem> Pull(Guid contactId)
        {
            var cart = GetCart(contactId);
            var cartItems = cart?.GetAllLineItems();
            return cartItems?.Select(item => new CartItem(item, contactId.ToString()));
        }

        public CartItem Update(Guid contactId, CartItem cartItem)
        {
            var cart = GetCart(contactId);
            if (cart == null || cartItem == null)
            {
                return null;
            }
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
                cart.AddLineItem(updatedItem);
            }

            _orderRepository.Save(cart);
            return new CartItem(updatedItem, contactId.ToString());
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
                return result;
            }

            return false;
        }

        public Total GetTotals(Guid contactId)
        {
            var cart = GetCart(contactId);
            return new Total(cart);
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
            return cart.GetFirstForm().Shipments.Select(s => new ShippingMethod(s));
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
    }
}
