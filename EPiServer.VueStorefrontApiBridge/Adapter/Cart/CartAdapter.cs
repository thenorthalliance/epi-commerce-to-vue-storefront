using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.ApiModel.Cart;
using Mediachase.Commerce.Customers;
using PaymentMethod = EPiServer.VueStorefrontApiBridge.ApiModel.Cart.PaymentMethod;

namespace EPiServer.VueStorefrontApiBridge.Adapter.Cart
{
    public class CartAdapter : ICartAdapter
    {
        private readonly IOrderRepository _orderRepository = ServiceLocator.Current.GetInstance<IOrderRepository>();
        public string CreateCart(string userId)
        {
            var cart = _orderRepository.LoadOrCreateCart<ICart>(GetUserGuid(userId), "Default");
            _orderRepository.Save(cart);
            return cart.Name;
        }

        public IEnumerable<CartItem> Pull(string userId, string cartId)
        {
            var cart = GetCart(userId, cartId);
            var cartItems = cart?.GetAllLineItems();
            return cartItems?.Select(item => new CartItem(item, cartId));
        }

        public CartItem Update(string userId, string cartId, CartItem cartItem)
        {
            var cart = GetCart(userId, cartId);
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
            return new CartItem(updatedItem, cartId);
        }

        public bool Delete(string userId, string cartId, CartItem cartItem)
        {
            var cart = GetCart(userId, cartId);
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

        public Total GetTotals(string userId, string cartId)
        {
            var cart = GetCart(userId, cartId);
            return new Total(cart);
        }

        public IEnumerable<PaymentMethod> GetPaymentMethods(string userId, string cartId)
        {
            var cart = GetCart(userId, cartId);
            return cart.GetFirstForm().Payments.Select(payment =>
                new PaymentMethod {Code = payment.PaymentMethodName.Replace(" ", "").ToLower(), Title = payment.PaymentMethodName});
        }

        public IEnumerable<ShippingMethod> GetShippingMethods(string userId, string cartId, UserAddressModel address)
        {
            var cart = GetCart(userId, cartId);
            return cart.GetFirstForm().Shipments.Select(s => new ShippingMethod(s));
        }

        private ICart GetCart(string userId, string cartId)
        {
            return _orderRepository.Load<ICart>(GetUserGuid(userId), cartId).FirstOrDefault();
        }

        private static Guid GetUserGuid(string userId)
        {
            return userId == null ? CustomerContext.Current.CurrentContactId : new Guid(userId);
        }
    }
}
