using System.Linq;
using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;
using EPiServer.Core;

namespace DataMigration.Mapper.Category
{
    public class CategoryMapper : IMapper
    {
        public Entity Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiCategory source))
            {
                return null;
            }

            var isPublished = source.Category.Status.Equals(VersionStatus.Published);
            return new Output.ElasticSearch.Entity.Category.Model.Category()
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
                Children = source.Children.Select(Map),
                ChildrenCount = source.Children.Count().ToString(),
                ProductCount = source.ProductsCount
            };
        }

        private static string GetDescription(EpiCategory category)
        {
            return category.Category.GetType().GetProperty("Description")?.GetValue(category.Category, null)?.ToString();
        }
    }
}