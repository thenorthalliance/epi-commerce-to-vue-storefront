using System;
using System.Collections.Generic;
using System.Linq;
using DataMigration.Input.Episerver;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Mapper;
using DataMigration.Output.ElasticSearch.Entity.Category.Model;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;
using DataMigration.Output.ElasticSearch.Service;
using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;
using Attribute = DataMigration.Output.ElasticSearch.Entity.Attribute.Model.Attribute;

namespace DataMigration.ScheduledJobs
{
    [ScheduledPlugIn(DisplayName = "Export to Vue Storefront")]
    public class VueStorefrontExportJob : ScheduledJobBase
    {
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private readonly ReferenceConverter _referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();
        private readonly IndexApiService _indexApiService = ServiceLocator.Current.GetInstance<IndexApiService>();

        public override string Execute()
        {
            if (!_indexApiService.IsServiceAvailable())
            {
                throw new Exception("Elasticsearch not available.");
            }
            if (!_indexApiService.CreateIndex())
            {
                throw new Exception("Unable to create Elasticsearch index.");
            }

            var catalogContentLink = _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()).First().ContentLink;

            var result = MigrateEntities<Attribute>(catalogContentLink)
                && MigrateEntities<Category>(catalogContentLink)
                && MigrateEntities<Product>(catalogContentLink);

            if (!result)
            {
               throw new Exception("Problem with indexing the data.");
            }

            return _indexApiService.ApplyChanges() ? "Success" : throw new Exception("Unable to update Elasticsearch index alias.");
        }

        private bool MigrateEntities<T>(ContentReference catalogReference) where T : class
        {
            var entities = GetMappedEntities<T>(catalogReference).ToList();
            var result = _indexApiService.IndexMany(entities);
            return result == entities.Count;
        }

        private static IEnumerable<T> GetMappedEntities<T>(ContentReference catalogReference) where T : class
        {
            var mapper = MapperFactory.Create<T>();
            return GetEntities<T>(catalogReference).Select(mapper.Map);
        }

        private static IEnumerable<CmsObjectBase> GetEntities<T>(ContentReference catalogReference) where T : class
        {
            var contentService = ContentServiceFactory.Create<T>();
            return contentService.GetAll(catalogReference, ContentLanguage.PreferredCulture);
        }
    }
}
