using System.Configuration;
using AutoMapper;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Reference.Commerce.VsfIntegration.Adapter;
using EPiServer.Reference.Commerce.VsfIntegration.Mapping;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.ApiBridge;
using EPiServer.Vsf.ApiBridge.AutoMapper;
using EPiServer.Vsf.Core;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using EPiServer.Vsf.Core.AutoMapper;
using EPiServer.Vsf.DataExport;
using EPiServer.Vsf.DataExport.Configuration;
using EPiServer.Vsf.DataExport.Mapping;

namespace EPiServer.Reference.Commerce.VsfIntegration
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class InitializationModule : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {}

        public void Uninitialize(InitializationEngine context)
        {}

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;

            InitializeMappings(services);
            CoreBootstrapper.Initialize(services);
            ApiBridgeBootstrapper.Initialize(services);

            var vsfExporterConfiguration = ConfigurationManager.GetSection("vsf.export") as VsfExporterConfiguration;
            var vsfApiBridgeConfiguration = ConfigurationManager.GetSection("vsf.apiBridge") as VsfApiBridgeConfiguration;

            services.RegisterVueStorefrontExporterDefaultService<QuicksilverVsfProduct>(vsfExporterConfiguration);
            services.RegisterVueStorefrontBridgeDefaultService(vsfApiBridgeConfiguration);

            services.AddTransient<IProductMapper<QuicksilverVsfProduct>, QuicksilverProductMapper>();
            services.AddTransient<ICategoryMapper, QuicksilverNodeMapper>();

            services.AddTransient<IUserAdapter<VsfUser>, QuickSilverUserAdapter>();
            services.AddTransient<ICartAdapter, QuickSilverCartAdapter>();
            services.AddTransient<IStockAdapter, QuickSilverStockAdapter>();
            services.AddTransient<IOrderAdapter, QuickSilverOrderAdapter>();
        }

        public static IServiceConfigurationProvider InitializeMappings(IServiceConfigurationProvider services)
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PayPalDtoMappingProfile>();
                cfg.AddProfile<PayPalDomainMappingProfile>();
            });

            services.AddSingleton(s => mapperConfiguration.CreateMapper());

            return services;
        }
    }
}
