using Nest;

namespace DataMigration.Output.Model
{
    public class Option
    {
        [PropertyName("label")]
        public string Name { get; set; }

        [Keyword(Name = "value")]
        public string Value { get; set; }
    }
}