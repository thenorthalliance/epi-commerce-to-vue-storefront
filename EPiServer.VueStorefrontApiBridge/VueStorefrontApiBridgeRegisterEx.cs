using System.IdentityModel.Tokens;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.Adapter.Invoice;
using EPiServer.VueStorefrontApiBridge.Adapter.Cart;
using EPiServer.VueStorefrontApiBridge.Authorization;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;
using EPiServer.VueStorefrontApiBridge.Manager.Address;
using EPiServer.VueStorefrontApiBridge.Manager.Contact;
using EPiServer.VueStorefrontApiBridge.Manager.User;
using EPiServer.VueStorefrontApiBridge.Mapper.User;

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

            if (!context.Services.Contains(typeof(IUserMapper)))
                context.Services.Add(typeof(IUserMapper), typeof(DefaultUserMapper), ServiceInstanceScope.Singleton);

            if (!context.Services.Contains(typeof(IUserManager)))
                context.Services.Add(typeof(IUserManager), typeof(DefaultUserManager), ServiceInstanceScope.Transient);

            if (!context.Services.Contains(typeof(ICustomerContactManager)))
                context.Services.Add(typeof(ICustomerContactManager), typeof(DefaultCustomerContactManager), ServiceInstanceScope.Transient);

            if (!context.Services.Contains(typeof(ICustomerAddressManager)))
                context.Services.Add(typeof(ICustomerAddressManager), typeof(DefaultCustomerAddressManager), ServiceInstanceScope.Transient);

            context.Services.Add(typeof(IInvoiceAdapter), typeof(MockedInvoiceAdapter), ServiceInstanceScope.Singleton);

            if (!context.Services.Contains(typeof(ICartAdapter)))
            {
                context.Services.Add(typeof(ICartAdapter), typeof(CartAdapter), ServiceInstanceScope.Transient);
            }
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
