using EPiServer.Vsf.Core.ApiBridge.Model.Order;

namespace EPiServer.Vsf.Core.ApiBridge.Adapter
{
    public interface IOrderAdapter
    {
        void CreateOrder(OrderRequestModel request);
        OrderHistoryModel GetOrders(string userId);
    }
}
