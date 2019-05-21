using System.Collections.Generic;
using Nest;

namespace EPiServer.Vsf.DataExport.Model
{
    //needed for single product calls, we need to index variants as simple products
    public class VsfSimpleProduct : VsfBaseProduct
    {
        [Keyword(Name = "color_options")]
        public IEnumerable<string> ColorOptions { get; set; }

        [Keyword(Name = "size_options")]
        public IEnumerable<string> SizeOptions { get; set; }
    }
}
