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
            //TODO this should happen outside this class 
            ValueIndex = variantProperty.AsAttributeValue(); 
        }

        [PropertyName("label")]
        public string Label { get; set; }

        [PropertyName("default_label")]
        public string DefaultLabel { get; set; }

        [PropertyName("order")]
        public int Order { get; set; }

        [Keyword(Name = "value_index")]
        public string ValueIndex { get; set; }

//        [JsonProperty("value_data", NullValueHandling = NullValueHandling.Ignore)]
//        [Text(Name = "value_data")]
//        public string ValueData { get; set; }
    }
}