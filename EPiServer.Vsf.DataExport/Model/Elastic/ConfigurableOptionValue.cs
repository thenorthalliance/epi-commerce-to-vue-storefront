using Nest;

namespace EPiServer.Vsf.DataExport.Model.Elastic
{
    public class ConfigurableOptionValue
    {
        [PropertyName("label")]
        public string Label { get; set; }

        [PropertyName("default_label")]
        public string DefaultLabel { get; set; }

        [PropertyName("order")]
        public int Order { get; set; }

        [Keyword(Name = "value_index")]
        public string ValueIndex { get; set; }
    }
}