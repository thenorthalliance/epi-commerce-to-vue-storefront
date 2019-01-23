using System.IdentityModel.Tokens;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.Authorization;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;

namespace EPiServer.VueStorefrontApiBridge
{
    [InitializableModule]
    public class ModuleWithServices : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            //MOCKED AUTH SETUP
            //THIS WILL CHANGE 
            context.Services.Add(typeof(IUserTokenProvider), new JwtUserTokenProvider(new AuthTokenOptions
            {
                Issuer = "test_issuer",
                Audience = "http://localhost:50244",
                SecurityKey = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes("alamakotaalamakotaalamakotaalamakota"))
            }, new MemoryRefreshTokenRepo()));
        }

        public void Initialize(InitializationEngine context)
        {}

        public void Uninitialize(InitializationEngine context)
        {}
    }

    public static class VueStorefrontApiBridgeRegisterEx
    {
        public static void RegisterVueStorefrontBridge(this HttpConfiguration configuration)
        {
            configuration.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            configuration.Routes.MapHttpRoute("VueStorefron API Bridge",
                "vsbridge/{controller}/{action}");
            
            configuration.Filters.Add(new VsfAuthentication());
        }
    }
}
