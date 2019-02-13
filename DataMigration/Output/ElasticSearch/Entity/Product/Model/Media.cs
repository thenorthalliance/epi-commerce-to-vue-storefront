using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class Media
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("pos")]
        public int Position { get; set; }

        [JsonProperty("typ")]
        public string Type { get; set; }

        [JsonProperty("lab")]
        public string Label { get; set; }
    }
}