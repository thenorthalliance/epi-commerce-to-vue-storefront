using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;
using EPiServer.Vsf.Core.Models.PayPal;
using Order = EPiServer.Vsf.Core.Models.PayPal.Orders.Order;

namespace EPiServer.Vsf.Core.ApiBridge.Adapter
{
    public interface IOrderAdapter
    {
        OrderResponseModel CreateOrder(OrderRequestModel request);
        OrderHistoryModel GetOrders(string userId);
        Task<Order> CreatePaypalOrder(PayPalCreateOrder request);
        Task<Order> AuthorizePaypalOrder(PayPalCaptureRequest request);
    }
}
