using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace DataMigration.Input.Episerver.Category.Service
{
    public class CategoryService: ContentService
    {
        public override IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level)
        {
            return LoadChildrenBatched<NodeContent>(parentReference, cultureInfo)
                .Select((nodeContent, index) => new EpiCategory
                {
                    Category = nodeContent,
                    Children = (IEnumerable<EpiCategory>) GetAll(nodeContent.ContentLink, cultureInfo, level + 1),
                    SortOrder = index,
                    Level = level,
                    ProductsCount = CountProductsUnderCategory(nodeContent.ContentLink, cultureInfo) //TODO in higher level it's 0, why?
                })
                .ToList();
        }

        private int CountProductsUnderCategory(ContentReference categorryReference, CultureInfo cultureInfo)
        {
            var products = GetEntriesRecursive<ProductContent>(categorryReference, cultureInfo);
            return products.Sum(product => ListVariations(product.ContentLink).Count());
        }

        private static IEnumerable<ProductVariation> ListVariations(ContentReference referenceToProduct)
        {
            var relationRepository = ServiceLocator.Current.GetInstance<IRelationRepository>();
            var variations = relationRepository.GetChildren<ProductVariation>(referenceToProduct);
            return variations;
        }
    }
}
