using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class ConfigurableOptionValue
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("default_label")]
        public string DefaultLabel { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("value_index")]
        public string ValueIndex { get; set; }

        [JsonProperty("value_data")]
        public string ValueData { get; set; }
    }
}