using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Category.Model
{
    public class EpiCategory : CmsObjectBase
    {
        public EpiCategory(NodeContent nodeContent, int sortOrder, int level, IEnumerable<EpiCategory> children, CultureInfo cultureInfo)
        {
            Category = nodeContent;
            Children = children;
            SortOrder = sortOrder;
            Level = level;
            ProductsCount = CountProductsUnderCategory(nodeContent.ContentLink, cultureInfo);
        }

        private static int CountProductsUnderCategory(ContentReference categoryReference, CultureInfo cultureInfo)
        {
            var products = ContentHelper.GetEntriesRecursive<ProductContent>(categoryReference, cultureInfo);
            var childCategories = ContentHelper.GetEntriesRecursive<NodeContent>(categoryReference, cultureInfo);
            var childCategoriesProductsCount = childCategories
                .Sum(cat => (ContentHelper.GetEntriesRecursive<ProductContent>(cat.ContentLink, cultureInfo)
                .Sum(prod => prod.GetVariants().Count())));
            return products.Sum(product => product.GetVariants().Count()) + childCategoriesProductsCount;
        }

        public override EntityType EntityType => EntityType.Category;
        public NodeContent Category { get; set; }
        public IEnumerable<EpiCategory> Children { get; set; }
        public int Level { get; set; }
        public int SortOrder { get; set; }
        public int ProductsCount { get; set; }
        public new int Id => Category.ContentLink.ID;
    }
}