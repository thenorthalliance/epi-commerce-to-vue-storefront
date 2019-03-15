using System.Linq;
using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Model;
using EPiServer.Core;

namespace DataMigration.Mapper.Category
{
    public class CategoryMapper : IMapper<Output.ElasticSearch.Entity.Category.Model.Category>
    {
        public Output.ElasticSearch.Entity.Category.Model.Category Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiCategory source))
                return null;

            var isPublished = source.Category.Status.Equals(VersionStatus.Published);

            return new Output.ElasticSearch.Entity.Category.Model.Category
            {
                Id = source.Id,
                Name = source.Category.DisplayName,
                AvailableSortBy = null,
                ParentId = source.Category.ParentLink.ID,
                Description = GetDescription(source),
                IsActive = isPublished,
                IncludeInMenu = isPublished,
                UrlKey = source.Category.RouteSegment.Replace("-", ""),
                Position = source.SortOrder,
                Level = source.Level,
                Children = source.Children.Select(MapCategory),
                ChildrenCount = source.Children.Count().ToString(),
                ProductCount = source.ProductsCount
            };
        }

        private static Output.ElasticSearch.Entity.Category.Model.Category MapCategory(EpiCategory epiCategory)
        {
            var isPublished = epiCategory.Category.Status.Equals(VersionStatus.Published);

            var category = new Output.ElasticSearch.Entity.Category.Model.Category
            {
                Id = epiCategory.Id,
                Name = epiCategory.Category.DisplayName,
                AvailableSortBy = null,
                ParentId = epiCategory.Category.ParentLink.ID,
                Description = GetDescription(epiCategory),
                IsActive = isPublished,
                IncludeInMenu = isPublished,
                UrlKey = epiCategory.Category.RouteSegment.Replace("-", ""), //TODO we repeat this a lot :)
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