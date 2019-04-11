using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.Vsf.Core.ApiBridge.Endpoint
{
    public interface IStockEndpoint
    {
        Task<VsfResponse> Check(string sku);
    }
}