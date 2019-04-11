using System;
using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Cart;


namespace EPiServer.VueStorefrontApiBridge.Endpoints
{
    public interface ICartEndpoint
    {
        Task<VsfResponse> CreateCart();
        Task<VsfResponse> PaymentMethods(Guid cartId);
        Task<VsfResponse> Pull(Guid cartId);
        Task<VsfResponse> Update(Guid cartId, [FromBody] CartRequest request);
        Task<VsfResponse> Delete(Guid cartId, [FromBody] CartRequest request);
        Task<VsfResponse> Totals(Guid cartId);
        Task<VsfResponse> ShippingMethods(Guid cartId, [FromBody] ShipmentMethodRequest request);
        Task<VsfResponse> ShippingInformation(Guid cartId, [FromBody] ShippingInformationRequest request);
        Task<VsfResponse> CollectTotals(Guid cartId, CollectTotalsRequest request);
        Task<VsfResponse> ApplyCoupon(Guid cartId, string coupon);
        Task<VsfResponse> DeleteCoupon(Guid cartId);
        Task<VsfResponse> Coupon(Guid cartId);
    }
}