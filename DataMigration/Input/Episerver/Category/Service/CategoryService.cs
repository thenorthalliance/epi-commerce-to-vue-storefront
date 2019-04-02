using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Common.Service;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Category.Service
{
    public class CategoryService : IContentService<EpiCategory>
    {
        private readonly ContentHelper _contentHelper;

        public CategoryService(ContentHelper contentHelper)
        {
            _contentHelper = contentHelper;
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
            return _contentHelper.LoadChildrenBatched<NodeContent>(parentReference, cultureInfo)
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
            var products = _contentHelper.GetEntriesRecursive<ProductContent>(categoryReference, cultureInfo);
            var childCategories = _contentHelper.GetEntriesRecursive<NodeContent>(categoryReference, cultureInfo);
            var childCategoriesProductsCount = childCategories
                .Sum(cat => (_contentHelper.GetEntriesRecursive<ProductContent>(cat.ContentLink, cultureInfo)
                    .Sum(prod => prod.GetVariants().Count())));
            return products.Sum(product => product.GetVariants().Count()) + childCategoriesProductsCount;
        }
    }
}
