using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using DataMigration.Input.Episerver.Product.Model;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Product.Service
{
    public class ProductService : ContentService
    {
        public override IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo)
        {
            var categories = GetEntriesRecursive<NodeContent>(parentReference, cultureInfo);
            var resultProducts = new List<EpiProduct>();
            foreach (var category in categories)
            {
                var categoryProducts = GetEntriesRecursive<ProductContent>(category.ContentLink, cultureInfo)
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
