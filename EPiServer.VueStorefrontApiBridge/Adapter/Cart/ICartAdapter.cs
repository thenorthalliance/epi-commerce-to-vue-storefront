using System.Collections.Generic;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.ApiModel.Cart;

namespace EPiServer.VueStorefrontApiBridge.Adapter.Cart
{
    public interface ICartAdapter
    {
        string CreateCart(string userId);
        IEnumerable<CartItem> Pull(string userId, string cartId);
        CartItem Update(string userId, string cartId, CartItem cartItem);
        bool Delete(string userId, string cartId, CartItem cartItem);
        Total GetTotals(string userId, string cartId);
        IEnumerable<PaymentMethod> GetPaymentMethods(string userId, string cartId);
        IEnumerable<ShippingMethod> GetShippingMethods(string userId, string cartId, UserAddressModel address);
    }
}
