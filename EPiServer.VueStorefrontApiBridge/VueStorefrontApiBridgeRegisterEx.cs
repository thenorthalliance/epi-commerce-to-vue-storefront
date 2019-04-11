using System.IdentityModel.Tokens;
using System.Web.Http;
using System.Web.Http.Cors;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.Core.ApiBridge;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.VueStorefrontApiBridge.Adapter;
using EPiServer.VueStorefrontApiBridge.Authorization.Claims;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;
using EPiServer.VueStorefrontApiBridge.Authorization.Token;
using EPiServer.VueStorefrontApiBridge.Endpoints;
using EPiServer.VueStorefrontApiBridge.Utils;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace EPiServer.VueStorefrontApiBridge
{
    
    public static class VsfServicesExt
    {

        public static IServiceConfigurationProvider VsfRegisterAuthServices<TUser>(this IServiceConfigurationProvider services) 
            where TUser : UserModel
        {
            //MOCKED AUTH SETUP
            //THIS WILL CHANGE 
            services.Add(typeof(IUserTokenProvider), new JwtUserTokenProvider(new AuthTokenOptions
            {
                Issuer = "test_issuer",
                Audience = "http://localhost:50244",
                SecurityKey = "alamakotaalamakotaalamakotaalamakota".ToSymmetricSecurityKey()
            }, new MemoryRefreshTokenRepository()));
            
            if (!services.Contains(typeof(ICartAdapter)))
                services.Add(typeof(ICartAdapter), typeof(CartAdapter), ServiceInstanceScope.Transient);

            if(!services.Contains(typeof(IStockAdapter)))
                services.Add(typeof(IStockAdapter), typeof(StockAdapter), ServiceInstanceScope.Transient);

            if (!services.Contains(typeof(IUserEndpoint)))
                services.Add(typeof(IUserEndpoint), typeof(UserEndpoint<TUser>), ServiceInstanceScope.Transient);

            if (!services.Contains(typeof(ICartEndpoint)))
                services.Add(typeof(ICartEndpoint), typeof(CartEndpoint), ServiceInstanceScope.Transient);

            if (!services.Contains(typeof(IStockEndpoint)))
                services.Add(typeof(IStockEndpoint), typeof(StockEndpoint), ServiceInstanceScope.Transient);
            
            services.Add(typeof(IUserClaimsProvider<TUser>), typeof(UserClaimsProvider<TUser>), ServiceInstanceScope.Transient);

            return services;
        }

        public static IServiceConfigurationProvider VsfRegisterUserManager<TUser, TUserManager>(this IServiceConfigurationProvider services) where TUser : UserModel
        {
            if (!services.Contains(typeof(IUserAdapter<TUser>)))
                services.Add(typeof(IUserAdapter<TUser>), typeof(TUserManager), ServiceInstanceScope.Transient);

            return services;
        }
    }


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
