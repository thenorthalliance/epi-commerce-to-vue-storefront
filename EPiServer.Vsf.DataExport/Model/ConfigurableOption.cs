using System.Collections.Generic;
using Nest;

namespace EPiServer.Vsf.DataExport.Model
{
    public class ConfigurableOption
    {
        [PropertyName("id")]
        public int Id { get; set; }

        [PropertyName("attribute_code")]
        public string AttributeCode { get; set; }

        [PropertyName("product_id")]
        public int ProductId { get; set; }

        [PropertyName("label")]
        public string Label { get; set; }

        [PropertyName("position")]
        public int Position { get; set; }

        [PropertyName("frontend_label")]
        public string FrontentLabel { get; set; }

        [PropertyName("values")]
        public List<ConfigurableOptionValue> Values { get; set; }
    }
}