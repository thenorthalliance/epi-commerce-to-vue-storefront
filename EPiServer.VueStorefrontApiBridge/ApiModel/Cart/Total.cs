using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class Total
    {
        [JsonProperty("grand_total")]
        public decimal GrandTotal { get; set; }

        [JsonProperty("weee_tax_applied_amount")]
        public string WeeeTaxAppliedAmount { get; set; }

        [JsonProperty("base_currency_code")]
        public string BaseCurrencyCode { get; set; }

        [JsonProperty("quote_currency_code")]
        public string QuoteCurrencyCode { get; set; }

        [JsonProperty("items_qty")]
        public decimal ItemsQty { get; set; }

        [JsonProperty("items")]
        public List<TotalItem> Items { get; set; }

        [JsonProperty("total_segments")]
        public List<TotalSegment> TotalSegments { get; set; }

    }
}
