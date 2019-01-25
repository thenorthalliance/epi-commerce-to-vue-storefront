using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataMigration.Mapper.Category.Model
{
    public class CategoryBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("children_count")]
        public string ChildrenCount { get; set; }

        [JsonProperty("children_data")]
        public IEnumerable<CategoryBase> Children { get; set; }

        [JsonProperty("include_in_menu")]
        public bool IncludeInMenu { get; set; }

        [JsonProperty("url_key")]
        public string UrlKey { get; set; }

    }
    public class Category : CategoryBase
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("custom_url")]
        public string CustomUrl { get; set; }

        //1 is a root category
        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("product_count")]
        public int ProductCount { get; set; }

        [JsonProperty("available_sort_by")]
        public IEnumerable<string> AvailableSortBy { get; set; }
       
        [JsonProperty("url_path")]
        public string UrlPath { get; set; }
    }

}

