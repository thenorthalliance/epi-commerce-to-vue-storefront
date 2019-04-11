using System.IdentityModel.Tokens;
using System.Web.Http;
using System.Web.Http.Cors;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.ApiBridge.Authorization.Claims;
using EPiServer.Vsf.ApiBridge.Authorization.Model;
using EPiServer.Vsf.ApiBridge.Authorization.Token;
using EPiServer.Vsf.ApiBridge.Utils;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace EPiServer.Vsf.ApiBridge
{
    public static class VsfServicesExt
    {
        public static IServiceConfigurationProvider VsfRegisterServices(this IServiceConfigurationProvider services, 
            VsfApiBridgeServicesConfiguration<UserModel> serviceConfiguration)
        {
            return services.VsfRegisterServices<UserModel>(serviceConfiguration);
        }

        public static IServiceConfigurationProvider VsfRegisterServices<TUser>(this IServiceConfigurationProvider services, 
            VsfApiBridgeServicesConfiguration<TUser> serviceConfiguration) 
            where TUser : UserModel
        {
            //MOCKED AUTH SETUP
            //THIS WILL CHANGE 
            if (!services.Contains(typeof(IUserTokenProvider)))
                services.Add(typeof(IUserTokenProvider), new JwtUserTokenProvider(new AuthTokenOptions
                {
                    Issuer = "test_issuer",
                    Audience = "http://localhost:50244",
                    SecurityKey = "alamakotaalamakotaalamakotaalamakota".ToSymmetricSecurityKey()
                }, new MemoryRefreshTokenRepository()));

            if (!services.Contains(typeof(IUserAdapter<TUser>)))
                services.Add(typeof(IUserAdapter<TUser>), serviceConfiguration.UserAdapter, ServiceInstanceScope.Transient);

            if (!services.Contains(typeof(ICartAdapter)))
                services.Add(typeof(ICartAdapter), serviceConfiguration.CartAdapter, ServiceInstanceScope.Transient);

            if(!services.Contains(typeof(IStockAdapter)))
                services.Add(typeof(IStockAdapter), serviceConfiguration.StockAdapter, ServiceInstanceScope.Transient);


            if (!services.Contains(typeof(IUserEndpoint)))
                services.Add(typeof(IUserEndpoint), serviceConfiguration.UserEndpoint, ServiceInstanceScope.Transient);

            if (!services.Contains(typeof(ICartEndpoint)))
                services.Add(typeof(ICartEndpoint), serviceConfiguration.CartEndpoint, ServiceInstanceScope.Transient);

            if (!services.Contains(typeof(IStockEndpoint)))
                services.Add(typeof(IStockEndpoint), serviceConfiguration.StockEndpoint, ServiceInstanceScope.Transient);
            

            services.Add(typeof(IUserClaimsProvider<TUser>), serviceConfiguration.UserClaimsProvider, ServiceInstanceScope.Transient);

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
