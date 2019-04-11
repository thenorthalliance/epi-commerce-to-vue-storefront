using Nest;

namespace EPiServer.Vsf.DataExport.Model.Elastic
{
    public class Option
    {
        [PropertyName("label")]
        public string Name { get; set; }

        [Keyword(Name = "value")]
        public string Value { get; set; }
    }
}