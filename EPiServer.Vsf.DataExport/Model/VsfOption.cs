using Nest;

namespace EPiServer.Vsf.DataExport.Model
{
    public class VsfOption
    {
        [PropertyName("label")]
        public string Name { get; set; }

        [Keyword(Name = "value")]
        public string Value { get; set; }
    }
}