using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Requests;
using PayPalCaptureRequest = EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Requests.PayPalCaptureRequest;

namespace EPiServer.Vsf.ApiBridge.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IOrderEndpoint _orderEndpoint;

        public OrderController(IOrderEndpoint orderEndpoint)
        {
            _orderEndpoint = orderEndpoint;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] OrderRequestModel request)
        {
            return Ok(await _orderEndpoint.CreateOrder(request));
        }

        [HttpPost]
        [Route("vsbridge/order/payments/paypal/create")]
        public async Task<IHttpActionResult> CreatePaypalPayment([FromBody] PayPalCreateOrderRequest createOrderRequest)
        {
            var order = await _orderEndpoint.CreatePaypalOrder(createOrderRequest);
            return Ok(order);
        }

        [HttpPost]
        [Route("vsbridge/order/payments/paypal/execute")]
        public async Task<IHttpActionResult> ExecutePaypalPayment([FromBody] PayPalCaptureRequest authorizeRequest)
        {
            var order = await _orderEndpoint.AuthorizePaypalOrder(authorizeRequest);
            return Ok(order);
        }
    }
}