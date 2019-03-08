using DataMigration.Output.ElasticSearch.Entity.Attribute.Helper;
using EPiServer.Core;
using Nest;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class ConfigurableOptionValue
    {
        public ConfigurableOptionValue(PropertyData variantProperty, int order)
        {
            DefaultLabel = variantProperty.Value.ToString();
            Label = variantProperty.Value.ToString();
            Order = order;
            ValueIndex = AttributeHelper.Instance.GetAttributeOption(variantProperty.PropertyDefinitionID, Label).Value; 
        }

        [PropertyName("label")]
        public string Label { get; set; }

        [PropertyName("default_label")]
        public string DefaultLabel { get; set; }

        [PropertyName("order")]
        public int Order { get; set; }

        [PropertyName("value_index")]
        public int ValueIndex { get; set; }

//        [JsonProperty("value_data", NullValueHandling = NullValueHandling.Ignore)]
//        [Text(Name = "value_data")]
//        public string ValueData { get; set; }
    }
}