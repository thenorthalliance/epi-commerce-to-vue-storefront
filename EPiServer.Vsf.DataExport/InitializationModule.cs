using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.DataExport.Input;
using EPiServer.Vsf.DataExport.Input.Model;
using EPiServer.Vsf.DataExport.Input.Service;
using EPiServer.Vsf.DataExport.Mapper;
using EPiServer.Vsf.DataExport.Output.Model;
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
            services.AddTransient<IContentService<EpiProduct>, ProductService>();
            services.AddTransient<IContentService<EpiContentProperty>, PropertyService>();
            services.AddTransient<IContentService<EpiCategory>, CategoryService>();
            services.AddTransient<IMapper<EpiProduct, Product>, ProductMapper>();
            services.AddTransient<IMapper<EpiContentProperty, Attribute>, AttributeMapper>();
            services.AddTransient<IMapper<EpiCategory, Category>, CategoryMapper>();
        }
    }
}
