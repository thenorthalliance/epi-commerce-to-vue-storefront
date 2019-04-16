using System.Collections.Generic;
using EPiServer.Vsf.DataExport.Model;
using Nest;

namespace EPiServer.Reference.Commerce.VsfIntegration.Mapping
{
    public class FasionVsfProduct : VsfBaseProduct
    {
        [Keyword(Name = "color_options")]
        public IEnumerable<string> ColorOptions { get; set; }

        [Keyword(Name = "size_options")]
        public IEnumerable<string> SizeOptions { get; set; }
    }
}
