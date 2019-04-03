using DataMigration.Input;
using DataMigration.Input.Model;
using DataMigration.Input.Service;
using DataMigration.Mapper;
using DataMigration.Output.Model;
using DataMigration.Utils.Epi;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace DataMigration
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
            services.AddTransient<IContentService<EpiProduct>, ProductService>();
            services.AddTransient<IContentService<EpiContentProperty>, PropertyService>();
            services.AddTransient<IContentService<EpiCategory>, CategoryService>();
            services.AddTransient<IMapper<EpiProduct, Product>, ProductMapper>();
            services.AddTransient<IMapper<EpiContentProperty, Attribute>, AttributeMapper>();
            services.AddTransient<IMapper<EpiCategory, Category>, CategoryMapper>();
        }
    }
}
