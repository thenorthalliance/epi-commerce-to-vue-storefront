using System;
using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;

namespace EPiServer.Vsf.ApiBridge.Endpoints
{
    public class OrderEndpoint : IOrderEndpoint
    {
        private readonly IOrderAdapter _orderAdapter;

        public OrderEndpoint(IOrderAdapter orderAdapter)
        {
            _orderAdapter = orderAdapter;
        }

        public Task<VsfResponse> CreateOrder(OrderRequestModel request)
        {
            try
            {
                _orderAdapter.CreateOrder(request);

                return Task.FromResult((VsfResponse)new VsfSuccessResponse<string>("OK"));
            }
            catch (Exception)
            {
                return Task.FromResult((VsfResponse) new VsfErrorResponse("ERROR"));
            }
        }
    }
}