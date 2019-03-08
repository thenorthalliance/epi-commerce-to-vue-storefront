using Nest;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Attribute.Model
{
    public class Option
    {
        [PropertyName("label")]
        public string Name { get; set; }

        [PropertyName("value")]
        public int Value { get; set; }
    }
}