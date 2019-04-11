using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.ApiModel;

namespace EPiServer.VueStorefrontApiBridge.Endpoints
{
    public interface IStockEndpoint
    {
        Task<VsfResponse> Check(string sku);
    }
}