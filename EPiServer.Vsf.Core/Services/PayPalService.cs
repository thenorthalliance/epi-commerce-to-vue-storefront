using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using EPiServer.Vsf.Core.Models.PayPal;
using PayPalCheckoutSdk.Orders;
using PayPalCheckoutSdk.Payments;

namespace EPiServer.Vsf.Core.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IMapper _mapper;

        public PayPalService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<Models.PayPal.Orders.Order> CreateOrderAsync(PayPalCreateOrder createOrderRequest)
        {
            var request = new OrdersCreateRequest();

            OrderRequest orderRequest = new OrderRequest
            {
                Intent = "AUTHORIZE",
                ApplicationContext = new ApplicationContext
                {
                    BrandName = "MAKING WAVES",
                    UserAction = "PAY_NOW",
                    ShippingPreference = "NO_SHIPPING"
                },
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        Amount = new AmountWithBreakdown
                        {
                            CurrencyCode = createOrderRequest.Transaction.Amount.Currency,
                            Value = createOrderRequest.Transaction.Amount.Total.ToString(CultureInfo.InvariantCulture)
                        }
                    }
                }
            };

            request.Prefer("return=representation");
            request.RequestBody(orderRequest);
            var client = PayPalClient.Client(); //TODO: The client is crap, change
            var response = await Task.Run(() => client.Execute(request)).ConfigureAwait(false); //TODO: The client is crap, change
            var payPalOrder = response.Result<Order>();
            var result = _mapper.Map<Models.PayPal.Orders.Order>(payPalOrder);

            return result;
        }


        public async Task<Models.PayPal.Orders.Order> AuthorizeOrderAsync(string orderId)
        {
            var request = new OrdersAuthorizeRequest(orderId);

            request.Prefer("return=representation");
            request.RequestBody(new OrderActionRequest());
            var client = PayPalClient.Client(); //TODO: The client is crap, change
            var response = await Task.Run(() => client.Execute(request)).ConfigureAwait(false); //TODO: The client is crap, change

            var payPalOrder = response.Result<Order>();
            var result = _mapper.Map<Models.PayPal.Orders.Order>(payPalOrder);

            return result;
        }

        public async Task<Models.PayPal.Payments.PayPalCapture> CaptureAuthorizationAsync(string authorizationId)
        {
            var request = new AuthorizationsCaptureRequest(authorizationId);
            request.Prefer("return=representation");
            request.RequestBody(new CaptureRequest()); //TODO: The client is crap, change
            var response = await Task.Run(() => PayPalClient.Client().Execute(request)).ConfigureAwait(false); //TODO: The client is crap, change

            var payPalCapture = response.Result<PayPalCheckoutSdk.Payments.Capture>();
            var result = _mapper.Map<Models.PayPal.Payments.PayPalCapture>(payPalCapture);

            return result;
        }
    }
}
