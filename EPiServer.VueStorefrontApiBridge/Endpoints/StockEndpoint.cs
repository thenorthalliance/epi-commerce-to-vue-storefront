using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.Adapter;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.ApiModel.Stock;

namespace EPiServer.VueStorefrontApiBridge.Endpoints
{
    public class StockEndpoint : IStockEndpoint
    {
        private readonly IStockAdapter _stockAdapter;

        public StockEndpoint(IStockAdapter stockAdapter)
        {
            _stockAdapter = stockAdapter;
        }

        public async Task<VsfResponse> Check(string sku)
        {
            return new VsfSuccessResponse<StockCheck>(await _stockAdapter.Check(sku));
        }
    }
}