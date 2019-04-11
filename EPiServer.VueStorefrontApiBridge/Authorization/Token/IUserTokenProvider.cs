using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization.Token
{
    public interface IUserTokenProvider
    {
        Task<string> GenerateNewToken(IEnumerable<Claim> userClaims);
        Task<string> GenerateNewRefreshToken(IEnumerable<Claim> userClaims);

        Task<RefreshToken> GetRefreshToken(string refreshTokenId);
    }
}