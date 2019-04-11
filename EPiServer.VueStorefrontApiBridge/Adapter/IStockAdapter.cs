using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.ApiModel.Stock;

namespace EPiServer.VueStorefrontApiBridge.Adapter
{
    public interface IStockAdapter
    {
        Task<StockCheck> Check(string code);
    }
}