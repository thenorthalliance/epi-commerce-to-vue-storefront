using System;
using System.Threading.Tasks;
using AutoMapper;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Responses;
using EPiServer.Vsf.Core.Models.PayPal;
using PayPalCaptureRequest = EPiServer.Vsf.Core.Models.PayPal.PayPalCaptureRequest;

namespace EPiServer.Vsf.ApiBridge.Endpoints
{
    public class OrderEndpoint : IOrderEndpoint
    {
        private readonly IOrderAdapter _orderAdapter;
        private readonly IMapper _mapper;

        public OrderEndpoint(IOrderAdapter orderAdapter, IMapper mapper)
        {
            _orderAdapter = orderAdapter;
            _mapper = mapper;
        }

        public Task<VsfResponse> CreateOrder(OrderRequestModel request)
        {
            try
            {
                var response = _orderAdapter.CreateOrder(request);

                return Task.FromResult((VsfResponse)new VsfSuccessResponse<OrderResponseModel>(response));
            }
            catch (Exception e)
            {
                return Task.FromResult((VsfResponse) new VsfErrorResponse("ERROR"));
            }
        }

        public async Task<PayPalOrderResponse> CreatePaypalOrder(Core.ApiBridge.Model.Order.PayPal.Requests.PayPalCreateOrderRequest createOrderRequest)
        {
            var request = _mapper.Map<PayPalCreateOrder>(createOrderRequest);
            var paypalOrder = await _orderAdapter.CreatePaypalOrder(request);
            var apiOrder = _mapper.Map<PayPalOrderResponse>(paypalOrder);

            return apiOrder;
        }

        public async Task<PayPalOrderResponse> AuthorizePaypalOrder(Core.ApiBridge.Model.Order.PayPal.Requests.PayPalCaptureRequest authorizeRequest)
        {
            var request = _mapper.Map<PayPalCaptureRequest>(authorizeRequest);
            var paypalOrder = await _orderAdapter.AuthorizePaypalOrder(request);
            var apiOrder = _mapper.Map<PayPalOrderResponse>(paypalOrder);

            return apiOrder;
        }
    }
}