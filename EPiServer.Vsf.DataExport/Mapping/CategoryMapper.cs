using System.Linq;
using EPiServer.Core;
using EPiServer.Vsf.Core.Mapping;
using EPiServer.Vsf.DataExport.Model;

namespace EPiServer.Vsf.DataExport.Mapping
{
    public interface ICategoryMapper : IMapper<EpiCategory, VsfCategory>
    {}

    public class CategoryMapper : ICategoryMapper
    {
        public VsfCategory Map(EpiCategory source)
        {
            var isPublished = source.Category.Status.Equals(VersionStatus.Published);

            return new VsfCategory
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

        private static VsfCategory MapCategory(EpiCategory epiCategory)
        {
            var isPublished = epiCategory.Category.Status.Equals(VersionStatus.Published);

            var category = new VsfCategory
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