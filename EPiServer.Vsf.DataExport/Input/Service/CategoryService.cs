using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Vsf.DataExport.Input.Model;
using EPiServer.Vsf.DataExport.Utils.Epi;

namespace EPiServer.Vsf.DataExport.Input.Service
{
    public class CategoryService : IContentService<EpiCategory>
    {
        private readonly ContentService _contentService;

        public CategoryService(ContentService contentService)
        {
            _contentService = contentService;
        }

        public IEnumerable<EpiCategory> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2)
        {
            var categoryTree = GetCategoryTree(parentReference, cultureInfo).ToList();
            var catList = new List<EpiCategory>();
            catList.AddRange(categoryTree);
            foreach (var cat in categoryTree)
            {
                catList.AddRange(DisassembleTree(cat));
            }

            return catList;
        }

        private IEnumerable<EpiCategory> GetCategoryTree(ContentReference parentReference, CultureInfo cultureInfo,
            int level = 2)
        {
            return _contentService.LoadChildrenBatched<NodeContent>(parentReference, cultureInfo)
                .Select((nodeContent, index) => CreateEpiCategory(nodeContent, index, level, GetCategoryTree(nodeContent.ContentLink, cultureInfo, level + 1), cultureInfo))
                .ToList();
        }

        private static IEnumerable<EpiCategory> DisassembleTree(EpiCategory root)
        {
            var res = new List<EpiCategory>();
            foreach (var entity in root.Children)
            {
                var rootChild = entity;
                res.Add(rootChild);
                res.AddRange(DisassembleTree(rootChild));
            }

            return res;
        }

        private EpiCategory CreateEpiCategory(NodeContent nodeContent, int sortOrder, int level, IEnumerable<EpiCategory> children, CultureInfo cultureInfo)
        {
            return new EpiCategory
            {
                Category = nodeContent,
                Children = children,
                SortOrder = sortOrder,
                Level = level,
                ProductsCount = CountProductsUnderCategory(nodeContent.ContentLink, cultureInfo)
            };
        }

        private int CountProductsUnderCategory(ContentReference categoryReference, CultureInfo cultureInfo)
        {
            var products = _contentService.GetEntriesRecursive<ProductContent>(categoryReference, cultureInfo);
            var childCategories = _contentService.GetEntriesRecursive<NodeContent>(categoryReference, cultureInfo);
            var childCategoriesProductsCount = childCategories
                .Sum(cat => (_contentService.GetEntriesRecursive<ProductContent>(cat.ContentLink, cultureInfo)
                    .Sum(prod => prod.GetVariants().Count())));
            return products.Sum(product => product.GetVariants().Count()) + childCategoriesProductsCount;
        }
    }
}
