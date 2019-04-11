using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Cart
{
    public class TotalSegmentExtensionAttributes
    {
        [JsonProperty("tax_grandtotal_details")]
        public List<object> TaxGrandtotalDetails { get; set; }
    }
}
