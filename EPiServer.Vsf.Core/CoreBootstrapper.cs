using EPiServer.ServiceLocation;
using EPiServer.Vsf.Core.Services;

namespace EPiServer.Vsf.Core
{
    public class CoreBootstrapper
    {
        public static IServiceConfigurationProvider Initialize(IServiceConfigurationProvider services)
        {
            services.AddScoped<IPayPalService, PayPalService>();
            return services;
        }
    }
}
