using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EPiServer.Vsf.ApiBridge.Authorization.Model;

namespace EPiServer.Vsf.ApiBridge.Authorization.Token
{
    public interface IUserTokenProvider
    {
        Task<string> GenerateNewToken(IEnumerable<Claim> userClaims);
        Task<string> GenerateNewRefreshToken(IEnumerable<Claim> userClaims);
        Task<RefreshToken> GetRefreshToken(string refreshTokenId);
    }
}