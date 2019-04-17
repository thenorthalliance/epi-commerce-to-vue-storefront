using EPiServer.Core;
using EPiServer.Vsf.DataExport.Model;

namespace EPiServer.Vsf.DataExport.Utils
{
    public static class AttributeHelper
    {
        public static string AsAttributeValue(this PropertyData property)
        {
            return GetAttributeValue(property.PropertyDefinitionID, property.Value.ToString());
        }

        public static VsfOption GetAttributeOption(int attributeId, string attrLabel)
        {
            return new VsfOption
            {
                Name = attrLabel,
                Value = GetAttributeValue(attributeId, attrLabel)
            };
        }

        public static string GetAttributeValue(int attributeId, string attrLabel)
        {
            return $"{attributeId}_{attrLabel}";
        }
    }
}
