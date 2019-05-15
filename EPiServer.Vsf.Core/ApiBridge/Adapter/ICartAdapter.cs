using System;
using System.Collections.Generic;
using EPiServer.Commerce.Order;
using EPiServer.Vsf.Core.ApiBridge.Model.Cart;
using EPiServer.Vsf.Core.ApiBridge.Model.User;

namespace EPiServer.Vsf.Core.ApiBridge.Adapter
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
        void UpdateShippingMethod(Guid contactId, int shipmentId, Guid shippingMethodId);
        bool AddCoupon(Guid contactId, string couponCode);
        string GetCartCoupon(Guid contactId);
        bool DeleteCoupon(Guid contactId);
        ICart GetCart(Guid contactId); //TODO: REMOVE
    }
}
