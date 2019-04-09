using Nest;

namespace EPiServer.Vsf.DataExport.Output.Model
{
    public class Option
    {
        [PropertyName("label")]
        public string Name { get; set; }

        [Keyword(Name = "value")]
        public string Value { get; set; }
    }
}