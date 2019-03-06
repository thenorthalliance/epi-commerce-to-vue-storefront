using EPiServer.Core;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class ConfigurableOptionValue
    {
        public ConfigurableOptionValue(PropertyData variantProperty, int order)
        {
            DefaultLabel = variantProperty.Value.ToString();
            Label = variantProperty.Value.ToString();
            Order = order;
            ValueIndex = int.Parse(variantProperty.PropertyDefinitionID.ToString() + Order); //TODO filters displays on category page but not in the product page
        }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("default_label")]
        public string DefaultLabel { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("value_index")]
        public int ValueIndex { get; set; }

        [JsonProperty("value_data", NullValueHandling = NullValueHandling.Ignore)]
        public string ValueData { get; set; }
    }
}