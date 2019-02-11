using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class ConfigurableOption
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("frontend_label")]
        public string FrontentLabel { get; set; }

        [JsonProperty("values")]
        public List<ConfigurableOptionValue> Values { get; set; }
    }
}