using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Stock;

namespace EPiServer.Vsf.ApiBridge.Endpoints
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