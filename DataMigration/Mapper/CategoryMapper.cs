using System.Linq;
using DataMigration.Input.Model;
using DataMigration.Output.Model;
using EPiServer.Core;

namespace DataMigration.Mapper
{
    public class CategoryMapper : IMapper<EpiCategory, Category>
    {
        public Category Map(EpiCategory source)
        {
            var isPublished = source.Category.Status.Equals(VersionStatus.Published);

            return new Category
            {
                Id = source.Id,
                Name = source.Category.DisplayName,
                AvailableSortBy = null,
                ParentId = source.Category.ParentLink.ID,
                Description = GetDescription(source),
                IsActive = isPublished,
                IncludeInMenu = isPublished,
                UrlKey = source.Category.RouteSegment,
                Position = source.SortOrder,
                Level = source.Level,
                Children = source.Children.Select(MapCategory),
                ChildrenCount = source.Children.Count().ToString(),
                ProductCount = source.ProductsCount
            };
        }

        private static Category MapCategory(EpiCategory epiCategory)
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
                UrlKey = epiCategory.Category.RouteSegment,
                Position = epiCategory.SortOrder,
                Level = epiCategory.Level,
                Children = epiCategory.Children.Select(MapCategory),
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