using System.Linq;
using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity.Category.Model;
using EPiServer.Core;

namespace DataMigration.Mapper
{
    public class CategoryMapper : IMapper<Category>
    {
        public Category Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiCategory source))
                return null;

            return NewCategory(source);
        }

        private static Category NewCategory(EpiCategory epiCategory)
        {
            var isPublished = epiCategory.Category.Status.Equals(VersionStatus.Published);

            var category = new Category
            {
                Id = epiCategory.Id,
                Name = epiCategory.Category.DisplayName,
                AvailableSortBy = null,
                ParentId = epiCategory.Category.ParentLink.ID,
                Description = GetDescription(epiCategory),
                IsActive = isPublished,
                IncludeInMenu = isPublished,
                UrlKey = epiCategory.Category.RouteSegment.Replace("-", ""),
                Position = epiCategory.SortOrder,
                Level = epiCategory.Level,
                Children = epiCategory.Children.Select(NewCategory),
                ChildrenCount = epiCategory.Children.Count().ToString(),
                ProductCount = epiCategory.ProductsCount
            };

            return category;
        }

        private static string GetDescription(EpiCategory category)
        {
            return category.Category.GetType().GetProperty("Description")?.GetValue(category.Category, null)?.ToString();
        }
    }
}