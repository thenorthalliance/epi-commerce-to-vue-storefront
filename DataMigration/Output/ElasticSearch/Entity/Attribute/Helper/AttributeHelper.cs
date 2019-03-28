using DataMigration.Output.ElasticSearch.Entity.Attribute.Model;
using EPiServer.Core;

namespace DataMigration.Output.ElasticSearch.Entity.Attribute.Helper
{
    public static class AttributeHelper
    {
        public static string AsAttributeValue(this PropertyData property)
        {
            return GetAttributeValue(property.PropertyDefinitionID, property.Value.ToString());
        }

        public static Option GetAttributeOption(int attributeId, string attrLabel)
        {
            return new Option
            {
                Name = attrLabel,
                Value = GetAttributeValue(attributeId, attrLabel)
            };
        }

        public static string GetAttributeValue(int attributeId, string attrLabel)
        {
            //TODO Don't know if replacing spaces with '_' is necessary
            return $"{attributeId}_{attrLabel.Replace(" ", "_")}";
        }
    }
}
