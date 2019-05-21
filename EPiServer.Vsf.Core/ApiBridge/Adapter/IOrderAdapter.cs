using EPiServer.Vsf.Core.ApiBridge.Model.Order;

namespace EPiServer.Vsf.Core.ApiBridge.Adapter
{
    public interface IOrderAdapter
    {
        OrderResponseModel CreateOrder(OrderRequestModel request);
        OrderHistoryModel GetOrders(string userId);
    }
}
