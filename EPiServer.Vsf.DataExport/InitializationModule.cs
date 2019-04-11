using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.DataExport.Mapper;
using EPiServer.Vsf.DataExport.Model;
using EPiServer.Vsf.DataExport.Model.Elastic;
using EPiServer.Vsf.DataExport.Utils.Epi;

namespace EPiServer.Vsf.DataExport
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class InitializationModule : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;

            services.AddSingleton<ContentService>();
            services.AddSingleton<PriceService>();
            services.AddSingleton<InventoryService>();
            services.AddTransient<IMapper<ProductContent, Product>, ProductMapper>();
            services.AddTransient<IMapper<EpiContentProperty, Attribute>, AttributeMapper>();
            services.AddTransient<IMapper<EpiCategory, Category>, CategoryMapper>();
        }
    }
}
