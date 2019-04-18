using EPiServer.ServiceLocation;
using EPiServer.Vsf.Core.Exporting;
using EPiServer.Vsf.DataExport.Configuration;
using EPiServer.Vsf.DataExport.Exporting;
using EPiServer.Vsf.DataExport.Mapping;
using EPiServer.Vsf.DataExport.Model;
using EPiServer.Vsf.DataExport.Utils.Epi;

namespace EPiServer.Vsf.DataExport
{
    public static class VueStorefrontDataExportRegisterEx
    {
        public static void RegisterVueStorefrontExporterDefaultService<TProduct>(this IServiceConfigurationProvider services, VsfExporterConfiguration vsfExporterConfiguration) where TProduct: VsfBaseProduct
        {
            services.AddSingleton(vsfExporterConfiguration);

            services.AddTransient<IAttributeMapper, AttributeMapper>();

            services.AddTransient<IContentExtractor, ContentExtractor>();
            services.AddSingleton<IVsfPriceService, VsfPriceService>();

            services.AddTransient<IExtractedContentHandler, ExtractedContentHandler<TProduct>>();
            services.AddTransient<IIndexingService, IndexingService<TProduct>>();

            services.AddTransient<IContentLoaderWrapper, CachedContentLoaderWrapper>(sc => new CachedContentLoaderWrapper(new ContentLoaderWrapper(sc.GetInstance<IContentLoader>())));
        }
    }
}