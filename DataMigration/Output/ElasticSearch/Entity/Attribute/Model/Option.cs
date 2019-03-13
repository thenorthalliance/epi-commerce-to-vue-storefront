using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Attribute.Model
{
    public class Option
    {
        [JsonProperty("label")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}