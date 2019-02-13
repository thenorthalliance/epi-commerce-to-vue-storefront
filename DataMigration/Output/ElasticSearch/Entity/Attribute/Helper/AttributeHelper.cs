namespace DataMigration.Output.ElasticSearch.Entity.Attribute.Helper
{
    public class AttributeHelper
    {
        public static string CreateValueIndex(int attributeId, string value)
        {
            return string.Concat(attributeId, "_", value.Replace(" ", string.Empty));
        }
    }
}
