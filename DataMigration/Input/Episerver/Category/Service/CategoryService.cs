using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Helpers;
using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Category.Service
{
    public class CategoryService: ContentService
    {
        public override IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo)
        {
            return GetAll(parentReference, cultureInfo, 0);
        }

        private IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level)
        {
            return LoadChildrenBatched<NodeContent>(parentReference, cultureInfo)
                .Select((nodeContent, index) => new EpiCategory
                {
                    Category = nodeContent,
                    Children = (IEnumerable<EpiCategory>)GetAll(nodeContent.ContentLink, cultureInfo, level + 1),
                    SortOrder = index,
                    Level = level,
                    ProductsCount = CountProductsUnderCategory(nodeContent.ContentLink, cultureInfo) 
                })
                .ToList();
        }

        private int CountProductsUnderCategory(ContentReference categoryReference, CultureInfo cultureInfo)
        {
            var products = GetEntriesRecursive<ProductContent>(categoryReference, cultureInfo);
            var childCategories = GetEntriesRecursive<NodeContent>(categoryReference, cultureInfo);
            var childCategoriesProductsCount = childCategories
                .Sum(cat => (GetEntriesRecursive<ProductContent>(cat.ContentLink, cultureInfo)
                    .Sum(prod => ContentHelper.GetProductVariations(prod.ContentLink).Count())));
            return products.Sum(product => ContentHelper.GetProductVariations(product.ContentLink).Count()) + childCategoriesProductsCount;
        }
    }
}
