using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.VueStorefrontApiBridge.Endpoints
{
    public interface IStockEndpoint
    {
        Task<VsfResponse> Check(string sku);
    }
}