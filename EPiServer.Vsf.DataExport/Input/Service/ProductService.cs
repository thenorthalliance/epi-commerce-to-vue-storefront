using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Vsf.DataExport.Input.Model;
using EPiServer.Vsf.DataExport.Utils.Epi;

namespace EPiServer.Vsf.DataExport.Input.Service
{
    public class ProductService : IContentService<EpiProduct>
    {
        private readonly ContentService _contentService;

        public ProductService(ContentService contentService)
        {
            _contentService = contentService;
        }

        public IEnumerable<EpiProduct> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2)
        {
            var categories = _contentService.GetEntriesRecursive<NodeContent>(parentReference, cultureInfo);
            var resultProducts = new List<EpiProduct>();
            foreach (var category in categories)
            {
                var categoryProducts = _contentService.GetEntriesRecursive<ProductContent>(category.ContentLink, cultureInfo)
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
