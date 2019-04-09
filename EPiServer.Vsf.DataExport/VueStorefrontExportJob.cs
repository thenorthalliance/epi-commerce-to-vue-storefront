using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.DataExport.Input;
using EPiServer.Vsf.DataExport.Input.Model;
using EPiServer.Vsf.DataExport.Mapper;
using EPiServer.Vsf.DataExport.Output;
using EPiServer.Vsf.DataExport.Output.Model;
using Mediachase.Commerce.Catalog;
using Attribute = EPiServer.Vsf.DataExport.Output.Model.Attribute;

namespace EPiServer.Vsf.DataExport
{
    [ScheduledPlugIn(DisplayName = "Export to Vue Storefront")]
    public class VueStorefrontExportJob : ScheduledJobBase
    {
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private readonly ReferenceConverter _referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
        private readonly IndexApiService _indexApiService = ServiceLocator.Current.GetInstance<IndexApiService>();

        private int _itemsCount;

        public override string Execute()
        {
            _itemsCount = 0;

            if (!_indexApiService.IsServiceAvailable())
            {
                throw new Exception("Elasticsearch not available.");
            }
            if (!_indexApiService.CreateIndex())
            {
                throw new Exception("Unable to create Elasticsearch index.");
            }

            var catalogContentLink = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).First().ContentLink;

            var result = MigrateEntities<EpiContentProperty, Attribute>(catalogContentLink)
                && MigrateEntities<EpiCategory, Category>(catalogContentLink)
                && MigrateEntities<EpiProduct, Product>(catalogContentLink);

            if (!result)
            {
               throw new Exception("Problem with indexing the data.");
            }

            return _indexApiService.ApplyChanges() ? $"Success. {_itemsCount} items exported." : throw new Exception("Unable to update Elasticsearch index alias.");
        }

        private bool MigrateEntities<TSource, TDestination>(ContentReference catalogReference) where TSource : ICmsObject where TDestination : class
        {
            OnStatusChanged($"Exporting '{typeof(TDestination).Name}' data");
            var entities = GetMappedEntities<TSource, TDestination>(catalogReference).ToList();
            var result = _indexApiService.IndexMany(entities);
            _itemsCount += result;
            return result == entities.Count;
        }

        private static IEnumerable<TDestination> GetMappedEntities<TSource, TDestination>(ContentReference catalogReference) where TSource : ICmsObject where TDestination : class
        {
            var mapper = ServiceLocator.Current.GetInstance<IMapper<TSource, TDestination>>();
            return GetEntities<TSource>(catalogReference).Select(mapper.Map);
        }

        private static IEnumerable<T> GetEntities<T>(ContentReference catalogReference) where T : ICmsObject
        {
            var contentService = ServiceLocator.Current.GetInstance<IContentService<T>>();
            return contentService.GetAll(catalogReference, ContentLanguage.PreferredCulture);
        }
    }
}
