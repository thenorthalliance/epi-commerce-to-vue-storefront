using System.Threading.Tasks;
using EPiServer.Vsf.Core.Models.PayPal;
using EPiServer.Vsf.Core.Models.PayPal.Orders;

namespace EPiServer.Vsf.Core.Services
{
    public interface IPayPalService
    {
        Task<Order> CreateOrderAsync(PayPalCreateOrder request);

        Task<Order> AuthorizeOrderAsync(string payPalOrderId);

        Task<Models.PayPal.Payments.PayPalCapture> CaptureAuthorizationAsync(string authorizationId);
    }
}
