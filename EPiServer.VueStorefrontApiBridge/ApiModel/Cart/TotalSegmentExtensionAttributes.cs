using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class TotalSegmentExtensionAttributes
    {
        [JsonProperty("tax_grandtotal_details")]
        public List<object> TaxGrandtotalDetails { get; set; }
    }
}
