using System.Threading.Tasks;
using EPiServer.Vsf.ApiBridge.Authorization.Token.Model;

namespace EPiServer.Vsf.ApiBridge.Authorization.Token
{
    public interface IRefreshTokenRepository
    {
        Task StoreToken(RefreshToken token);
        Task<RefreshToken> GetToken(string id);
        Task RemoveToken(string id);
    }
}