using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class CategoryListItem
    {
        [JsonProperty("category_id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}