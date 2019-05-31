using System;
using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Cart;

namespace EPiServer.Vsf.Core.ApiBridge.Endpoint
{
    public interface ICartEndpoint
    {
        Task<VsfResponse> CreateCart();
        Task<VsfResponse> PaymentMethods(Guid cartId);
        Task<VsfResponse> Pull(Guid cartId);
        Task<VsfResponse> Update(Guid cartId, CartRequest request);
        Task<VsfResponse> Delete(Guid cartId, CartRequest request);
        Task<VsfResponse> Totals(Guid cartId);
        Task<VsfResponse> ShippingMethods(Guid cartId, ShipmentMethodRequest request);
        Task<VsfResponse> ShippingInformation(Guid cartId, ShippingInformationRequest request);
        Task<VsfResponse> EmptyCart(Guid cartId);
        Task<VsfResponse> CollectTotals(Guid cartId, CollectTotalsRequest request);
        Task<VsfResponse> ApplyCoupon(Guid cartId, string coupon);
        Task<VsfResponse> DeleteCoupon(Guid cartId);
        Task<VsfResponse> Coupon(Guid cartId);
    }
}