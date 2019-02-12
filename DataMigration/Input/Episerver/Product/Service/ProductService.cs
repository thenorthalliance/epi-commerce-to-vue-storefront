using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using DataMigration.Input.Episerver.Product.Model;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Product.Service
{
    public class ProductService : IContentService
    {
        public IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2)
        {
            var categories = ContentHelper.GetEntriesRecursive<NodeContent>(parentReference, cultureInfo);
            var resultProducts = new List<EpiProduct>();
            foreach (var category in categories)
            {
                var categoryProducts = ContentHelper.GetEntriesRecursive<ProductContent>(category.ContentLink, cultureInfo)
                    .Select(productContent => new EpiProduct
                    {
                        ProductContent = productContent
                    });
                resultProducts.AddRange(categoryProducts);
            }

            return resultProducts;
        }
    }
}
