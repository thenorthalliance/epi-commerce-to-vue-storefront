using System.Collections.Generic;
using EPiServer.Core;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Product.Model
{
    public class ConfigurableOption
    {
        public ConfigurableOption(PropertyData variantProperty, int position, int productId, List<ConfigurableOptionValue> values)
        {
            Id = variantProperty.PropertyDefinitionID;
            Position = position;
            Label = variantProperty.Name;
            AttributeCode = variantProperty.Name.ToLower().Replace(" ", "_");
            FrontentLabel = variantProperty.Name.ToLower();
            ProductId = productId;
            Values = values;
        }

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