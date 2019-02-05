using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization
{
    public interface IUserTokenProvider
    {
        Task<string> GenerateNewToken(UserModel user);
        Task<string> GenerateNewRefreshToken(UserModel user);
        Task<RefreshToken> GetRefreshToken(string refreshTokenId);

        bool ValidateToken(string token, out TokenValidationResult validationResult);
    }
}