using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Requests;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Responses;

namespace EPiServer.Vsf.Core.ApiBridge.Endpoint
{
    public interface IOrderEndpoint
    {
        Task<VsfResponse> CreateOrder(OrderRequestModel request);
        Task<PayPalOrderResponse> CreatePaypalOrder(PayPalCreateOrderRequest request);
        Task<PayPalOrderResponse> AuthorizePaypalOrder(PayPalCaptureRequest request);
    }
}