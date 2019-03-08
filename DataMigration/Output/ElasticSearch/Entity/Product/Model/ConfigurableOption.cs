using System.Collections.Generic;
using EPiServer.Core;
using Nest;

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

        [PropertyName("id")]
        public int Id { get; set; }

        [PropertyName("attribute_code")]
        public string AttributeCode { get; set; }

        [PropertyName("product_id")]
        public int ProductId { get; set; }

        [PropertyName("label")]
        public string Label { get; set; }

        [PropertyName("position")]
        public int Position { get; set; }

        [PropertyName("frontend_label")]
        public string FrontentLabel { get; set; }

        [PropertyName("values")]
        public List<ConfigurableOptionValue> Values { get; set; }
    }
}