using System;
using System.Collections.Generic;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.ApiModel.Cart;

namespace EPiServer.VueStorefrontApiBridge.Adapter.Cart
{
    public interface ICartAdapter
    {
        string CreateCart(Guid contactId);
        IEnumerable<CartItem> Pull(Guid contactId);
        CartItem Update(Guid contactId, CartItem cartItem);
        bool Delete(Guid contactId, CartItem cartItem);
        Total GetTotals(Guid contactId);
        IEnumerable<PaymentMethod> GetPaymentMethods(Guid contactId);
        IEnumerable<ShippingMethod> GetShippingMethods(Guid contactId, UserAddressModel address);

        bool AddCoupon(Guid contactId, string couponCode);
        string GetCartCoupon(Guid contactId);
        bool DeleteCoupon(Guid contactId);
    }
}
