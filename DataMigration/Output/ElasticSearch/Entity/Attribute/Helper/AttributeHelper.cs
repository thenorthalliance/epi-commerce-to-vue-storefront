using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Model;

namespace DataMigration.Output.ElasticSearch.Entity.Attribute.Helper
{
    public sealed class AttributeHelper
    {
        private static readonly IDictionary<int, ICollection<Option>> Attributes = new Dictionary<int, ICollection<Option>>(); 
        private static AttributeHelper _instance;

        public static AttributeHelper Instance => _instance ?? (_instance = new AttributeHelper());

        public Option GetAttributeOption(int attributeId, string attrLabel)
        {
            if (!Attributes.ContainsKey(attributeId))
            {
                var attributeValue = int.Parse(new StringBuilder(attributeId + "1").ToString());
                var option = new Option {Name = attrLabel, Value = attributeValue};
                Attributes.Add(attributeId, new List<Option> {option});
                return option;
            }
            else
            {
                var options = Attributes[attributeId];
                var existingOption = options.FirstOrDefault(x => x.Name.Equals(attrLabel));
                if (existingOption != null)
                {
                    return existingOption;
                }
                var attributeValue = int.Parse(new StringBuilder(attributeId + (options.Count + 1).ToString()).ToString());
                var option = new Option { Name = attrLabel, Value = attributeValue };
                options.Add(option);
                Attributes[attributeId] = options;
                return option;
            }
        }
    }
}
