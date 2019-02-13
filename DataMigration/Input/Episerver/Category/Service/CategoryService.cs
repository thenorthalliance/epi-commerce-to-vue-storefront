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
            return ContentHelper.LoadChildrenBatched<NodeContent>(parentReference, cultureInfo)
                .Select((nodeContent, index) => new EpiCategory(nodeContent, index, level, (IEnumerable<EpiCategory>)GetAll(nodeContent.ContentLink, cultureInfo, level + 1), cultureInfo))
                .ToList();
        }
    }
}
