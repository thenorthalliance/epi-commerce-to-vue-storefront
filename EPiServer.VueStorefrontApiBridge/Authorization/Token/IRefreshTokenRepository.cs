using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization.Token
{
    public interface IRefreshTokenRepository
    {
        Task StoreToken(RefreshToken token);

        Task<RefreshToken> GetToken(string id);
        Task RemoveToken(string id);
    }
}