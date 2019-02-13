using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Category.Model
{
    public class CategoryBase : Entity
    {
        [JsonProperty("parent_id")]
        public int ParentId { get; set; }
        
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("children_count")]
        public string ChildrenCount { get; set; }

        [JsonProperty("children_data")]
        public IEnumerable<Entity> Children { get; set; }

        [JsonProperty("include_in_menu")]
        public bool IncludeInMenu { get; set; }

        [JsonProperty("url_key")]
        public string UrlKey { get; set; }

    }
}