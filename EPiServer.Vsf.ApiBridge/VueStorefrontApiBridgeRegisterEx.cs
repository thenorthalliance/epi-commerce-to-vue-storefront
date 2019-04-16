using System.IdentityModel.Tokens;
using System.Web.Http;
using System.Web.Http.Cors;
using EPiServer.Vsf.ApiBridge.Authorization.Token;
using EPiServer.Vsf.ApiBridge.Utils;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace EPiServer.Vsf.ApiBridge
{
    public static class VueStorefrontApiBridgeRegisterEx
    {
        public static void RegisterVueStorefrontBridge(this HttpConfiguration configuration)
        {
            configuration.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            configuration.Routes.MapHttpRoute("VueStorefron API Bridge",
                "vsbridge/{controller}/{action}");
        }

        public static IAppBuilder RegisterVueStorefrontAuth(this IAppBuilder bulder)
        {
            bulder.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {   
                AuthenticationType = "VueStorefronToken",
                TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = "alamakotaalamakotaalamakotaalamakota".ToSymmetricSecurityKey(),
                    ValidIssuer = "test_issuer",
                    ValidAudience = "http://localhost:50244",
                    ValidateLifetime = true,
                },
                Provider = new VsfJwtBearerTokenProvider()
            });
            return bulder;
        }
    }
}
