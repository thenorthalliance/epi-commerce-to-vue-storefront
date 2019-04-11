using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.VueStorefrontApiBridge.Endpoints;

namespace EPiServer.VueStorefrontApiBridge.Controllers
{
    public class StockController : ApiController
    {
        private readonly IStockEndpoint _stockEndpoint;

        public StockController(IStockEndpoint stockEndpoint)
        {
            _stockEndpoint = stockEndpoint;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Check([FromUri]string sku)
        {
            return Ok(await _stockEndpoint.Check(sku));
        }
    }
}