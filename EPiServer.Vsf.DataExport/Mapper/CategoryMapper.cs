using System.Linq;
using EPiServer.Core;
using EPiServer.Vsf.DataExport.Model;
using EPiServer.Vsf.DataExport.Model.Elastic;

namespace EPiServer.Vsf.DataExport.Mapper
{
    public class CategoryMapper : IMapper<EpiCategory, Category>
    {
        public Category Map(EpiCategory source)
        {
            var isPublished = source.Category.Status.Equals(VersionStatus.Published);

            return new Category
            {
                Id = source.Category.ContentLink.ID,
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
                ProductCount = source.TotalProductsCount
            };
        }

        private static Category MapCategory(EpiCategory epiCategory)
        {
            var isPublished = epiCategory.Category.Status.Equals(VersionStatus.Published);

            var category = new Category
            {
                Id = epiCategory.Category.ContentLink.ID,
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
                ProductCount = epiCategory.TotalProductsCount
            };

            return category;
        }
        
        private static string GetDescription(EpiCategory category)
        {
            return category.Category.GetType().GetProperty("Description")?.GetValue(category.Category, null)?.ToString();
        }
    }
}