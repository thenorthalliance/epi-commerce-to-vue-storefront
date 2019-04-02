using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Category.Service;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Common.Service;
using DataMigration.Input.Episerver.ContentProperty.Model;
using DataMigration.Input.Episerver.ContentProperty.Service;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Input.Episerver.Product.Service;
using DataMigration.Mapper;
using DataMigration.Mapper.Attribute;
using DataMigration.Mapper.Category;
using DataMigration.Mapper.Product;
using DataMigration.Output.ElasticSearch.Entity.Category.Model;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace DataMigration.Infrastructure
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    public class DataExportInitialization : IConfigurableModule
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

            services.AddSingleton<ContentHelper>();
            services.AddSingleton<PriceService>();
            services.AddSingleton<InventoryService>();
            services.AddTransient<IContentService<EpiProduct>, ProductService>();
            services.AddTransient<IContentService<EpiContentProperty>, PropertyService>();
            services.AddTransient<IContentService<EpiCategory>, CategoryService>();
            services.AddTransient<IMapper<EpiProduct, Product>, ProductMapper>();
            services.AddTransient<IMapper<EpiContentProperty, Output.ElasticSearch.Entity.Attribute.Model.Attribute>, AttributeMapper>();
            services.AddTransient<IMapper<EpiCategory, Category>, CategoryMapper>();
        }
    }
}
