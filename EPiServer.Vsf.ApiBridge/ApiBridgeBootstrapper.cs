using EPiServer.ServiceLocation;

namespace EPiServer.Vsf.ApiBridge
{
    public class ApiBridgeBootstrapper
    {
        public static IServiceConfigurationProvider Initialize(IServiceConfigurationProvider services)
        {
            return services;
        }
    }
}
