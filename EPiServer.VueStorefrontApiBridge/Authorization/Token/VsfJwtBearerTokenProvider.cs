using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Provider;

namespace EPiServer.VueStorefrontApiBridge.Authorization.Token
{
    public class VsfJwtBearerTokenProvider : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            context.Token = TryGetHeaderToken(context) ?? TryGetQueryToken(context);
            return Task.CompletedTask;
        }

        private static string TryGetHeaderToken(BaseContext context)
        {
            var tokenSplit = context.Request.Headers.Get("Authorization")?.Split(' ');

            if (tokenSplit == null || tokenSplit.Length != 2 || tokenSplit[0] != "Bearer" || string.IsNullOrWhiteSpace(tokenSplit[1]))
                return null;

            return tokenSplit[1];
        }

        private static string TryGetQueryToken(BaseContext context)
        {
            var token = context.Request.Query.Get("token");
            return string.IsNullOrWhiteSpace(token) ? null : token;
        }
    }
}
