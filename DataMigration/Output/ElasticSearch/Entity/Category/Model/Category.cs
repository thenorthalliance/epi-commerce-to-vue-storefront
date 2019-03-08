using System.Collections.Generic;
using System.Linq;
using DataMigration.Input.Episerver.Category.Model;
using EPiServer.Core;
using Nest;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Category.Model
{
    public class Category : CategoryBase
    {
        public Category()
        {
        }
        public Category(EpiCategory epiCategory)
        {
            var isPublished = epiCategory.Category.Status.Equals(VersionStatus.Published);
            Id = epiCategory.Id;
            Name = epiCategory.Category.DisplayName;
            AvailableSortBy = null;
            ParentId = epiCategory.Category.ParentLink.ID;
            Description = GetDescription(epiCategory);
            IsActive = isPublished;
            IncludeInMenu = isPublished;
            UrlKey = epiCategory.Category.RouteSegment.Replace("-",""); //TODO menu sidebar doesn't work when dashes are, check it again
            Position = epiCategory.SortOrder;
            Level = epiCategory.Level;
            Children = epiCategory.Children.Select(x => new Category(x));
            ChildrenCount = epiCategory.Children.Count().ToString();
            ProductCount = epiCategory.ProductsCount;
        }

        private static string GetDescription(EpiCategory category)
        {
            return category.Category.GetType().GetProperty("Description")?.GetValue(category.Category, null)?.ToString();
        }

        [PropertyName("description")]
        public string Description { get; set; }

        [PropertyName("is_active")]
        public bool IsActive { get; set; }

        //1 is a root category
        [PropertyName("level")]
        public int Level { get; set; }

        [PropertyName("product_count")]
        public int ProductCount { get; set; }

        [PropertyName("available_sort_by")]
        public IEnumerable<string> AvailableSortBy { get; set; }
    }
}

