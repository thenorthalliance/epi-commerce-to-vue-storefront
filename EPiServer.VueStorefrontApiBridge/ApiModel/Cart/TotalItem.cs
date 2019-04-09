using EPiServer.Commerce.Order;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class TotalItem
    {
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("base_price")]
        public long BasePrice { get; set; }

        [JsonProperty("qty")]
        public long Qty { get; set; }

        [JsonProperty("row_total")]
        public long RowTotal { get; set; }

        [JsonProperty("base_row_total")]
        public long BaseRowTotal { get; set; }

        [JsonProperty("row_total_with_discount")]
        public long RowTotalWithDiscount { get; set; }

        [JsonProperty("tax_amount")]
        public long TaxAmount { get; set; }

        [JsonProperty("base_tax_amount")]
        public long BaseTaxAmount { get; set; }

        [JsonProperty("tax_percent")]
        public long TaxPercent { get; set; }

        [JsonProperty("discount_amount")]
        public long DiscountAmount { get; set; }

        [JsonProperty("base_discount_amount")]
        public long BaseDiscountAmount { get; set; }

        [JsonProperty("discount_percent")]
        public long DiscountPercent { get; set; }

        [JsonProperty("options")]
        public string Options { get; set; }

        [JsonProperty("weee_tax_applied_amount")]
        public object WeeeTaxAppliedAmount { get; set; }

        [JsonProperty("weee_tax_applied")]
        public object WeeeTaxApplied { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("product_option")]
        public ProductOption ProductOption { get; set; }
    }
}
