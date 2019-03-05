using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Helpers;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Category.Service
{
    public class CategoryService: IContentService 
    {
        public IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2)
        {
            var categoryTree = GetCategoryTree(parentReference, cultureInfo).ToList();
            var catList = new List<CmsObjectBase>();
            catList.AddRange(categoryTree);
            foreach (var cat in categoryTree)
            {
                catList.AddRange(DisassembleTree(cat as EpiCategory));
            }

            return catList;
        }

        private static IEnumerable<CmsObjectBase> GetCategoryTree(ContentReference parentReference, CultureInfo cultureInfo,
            int level = 2)
        {
            return ContentHelper.LoadChildrenBatched<NodeContent>(parentReference, cultureInfo)
                .Select((nodeContent, index) => new EpiCategory(nodeContent, index, level, (IEnumerable<EpiCategory>)GetCategoryTree(nodeContent.ContentLink, cultureInfo, level + 1), cultureInfo))
                .ToList();
        }

        private static IEnumerable<CmsObjectBase> DisassembleTree(EpiCategory root)
        {
            var res = new List<CmsObjectBase>();
            foreach (var entity in root.Children)
            {
                var rootChild = entity;
                res.Add(rootChild);
                res.AddRange(DisassembleTree(rootChild));
            }

            return res;
        }
    }
}
