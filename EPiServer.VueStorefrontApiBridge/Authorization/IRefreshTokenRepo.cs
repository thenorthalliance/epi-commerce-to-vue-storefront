using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization
{
    public interface IRefreshTokenRepo
    {
        Task StoreToken(RefreshToken token);

        Task<RefreshToken> GetToken(string id);
        Task RemoveToken(string id);
    }
}