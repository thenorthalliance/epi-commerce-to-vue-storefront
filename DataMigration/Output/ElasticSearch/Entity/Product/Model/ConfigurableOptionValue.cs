using DataMigration.Output.ElasticSearch.Entity.Attribute.Helper;
using EPiServer.Core;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class ConfigurableOptionValue
    {
        public ConfigurableOptionValue(PropertyData variantProperty)
        {
            DefaultLabel = variantProperty.Value.ToString();
            Label = variantProperty.Value.ToString();
            Order = 0;
            ValueIndex = AttributeHelper.CreateValueIndex(variantProperty.PropertyDefinitionID,
                variantProperty.Value.ToString());
        }

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