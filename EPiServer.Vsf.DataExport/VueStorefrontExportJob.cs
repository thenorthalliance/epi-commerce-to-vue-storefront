using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.DataExport.Mapper;
using EPiServer.Vsf.DataExport.Model;
using EPiServer.Vsf.DataExport.Model.Elastic;
using EPiServer.Vsf.DataExport.Utils.Elastic;
using EPiServer.Vsf.DataExport.Utils.Epi;
using Attribute = EPiServer.Vsf.DataExport.Model.Elastic.Attribute;

namespace EPiServer.Vsf.DataExport
{
    [ScheduledPlugIn(DisplayName = "Export to Vue Storefront")]
    public class VueStorefrontExportJob : ScheduledJobBase
    {
        private readonly IndexingService _indexingService = ServiceLocator.Current.GetInstance<IndexingService>();
        private readonly ContentService _contentService = ServiceLocator.Current.GetInstance<ContentService>();
        private readonly ContentPropertyLoader _epiContentPropertyLoader = ServiceLocator.Current.GetInstance<ContentPropertyLoader>();

        private int _itemsCount;

        public override string Execute()
        {
            _itemsCount = 0;
            _epiContentPropertyLoader.Clear();

            if (!_indexingService.IsServiceAvailable())
            {
                throw new Exception("Elasticsearch not available.");
            }
            if (!_indexingService.CreateIndex())
            {
                throw new Exception("Unable to create Elasticsearch index.");
            }

            var catalogContent = _contentService.GetRootCatalogs().First();
            ExportCategories(catalogContent.ContentLink, ContentLanguage.PreferredCulture);

            AddForIndexing<EpiContentProperty, Attribute>(_epiContentPropertyLoader.GetProperties());

            _itemsCount += _indexingService.IndexBatchAll();

            return _indexingService.ApplyChanges() ? $"Success. {_itemsCount} items exported." : throw new Exception("Unable to update Elasticsearch index alias.");
        }

        private IEnumerable<EpiCategory> ExportCategories(ContentReference contentReference, CultureInfo cultureInfo, int level = 2)
        {
            var result = new List<EpiCategory>();
            var index = 0;
            foreach (var nodeContent in _contentService.LoadChildrenBatched<NodeContent>(contentReference, cultureInfo))
            {
                var childCategories = ExportCategories(nodeContent.ContentLink, cultureInfo, level + 1);
                result.Add(ExportSingleCategory(nodeContent, index, level, childCategories, cultureInfo));
                index++;
            }

            return result;
        }

        private EpiCategory ExportSingleCategory(NodeContent nodeContent, int sortOrder, int level, IEnumerable<EpiCategory> children, CultureInfo cultureInfo)
        {
            var productsCount = ExportCategoryProducts(nodeContent.ContentLink, cultureInfo);

            var category = new EpiCategory
            {
                Category = nodeContent,
                Children = children,
                SortOrder = sortOrder,
                Level = level,
                CategoryProductsCount = productsCount
            };

            AddForIndexing<EpiCategory, Category>(category);

            _itemsCount += _indexingService.IndexBatchIfReady();

            return category;
        }

        private int ExportCategoryProducts(ContentReference contentReference, CultureInfo cultureInfo)
        {
            var products = _contentService.LoadChildrenBatched<ProductContent>(contentReference, cultureInfo).ToList();

            _epiContentPropertyLoader.LoadProperties(products);

            AddForIndexing<ProductContent, Product>(products);

            return products.Count;
        }

        private void AddForIndexing<TSource, TDestination>(TSource source) where TSource : class where TDestination : class
        {
            var mapper = ServiceLocator.Current.GetInstance<IMapper<TSource, TDestination>>();
            _indexingService.AddForIndexing(mapper.Map(source));
        }

        private void AddForIndexing<TSource, TDestination>(IEnumerable<TSource> source) where TSource : class where TDestination : class
        {
            var mapper = ServiceLocator.Current.GetInstance<IMapper<TSource, TDestination>>();
            _indexingService.AddForIndexing(source.Select(mapper.Map));
        }
    }
}
