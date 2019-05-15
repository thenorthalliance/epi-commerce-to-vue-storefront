using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;

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
    }
}