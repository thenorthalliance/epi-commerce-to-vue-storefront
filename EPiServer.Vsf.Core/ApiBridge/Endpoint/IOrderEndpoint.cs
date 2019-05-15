using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;

namespace EPiServer.Vsf.Core.ApiBridge.Endpoint
{
    public interface IOrderEndpoint
    {
        Task<VsfResponse> CreateOrder(OrderRequestModel request);
    }
}