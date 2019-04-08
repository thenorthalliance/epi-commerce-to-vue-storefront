using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.VueStorefrontApiBridge.Adapter.Stock;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.ApiModel.Stock;

namespace EPiServer.VueStorefrontApiBridge.Controllers
{
    public class StockController : ApiController
    {
        private readonly IStockAdapter _stockAdapter;

        public StockController(IStockAdapter stockAdapter)
        {
            _stockAdapter = stockAdapter;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Check([FromUri]string sku)
        {
            return Ok(new VsfSuccessResponse<StockCheck>(await _stockAdapter.Check(sku)));
        }
    }
}