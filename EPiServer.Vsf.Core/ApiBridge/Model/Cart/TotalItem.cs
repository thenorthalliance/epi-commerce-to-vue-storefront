using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Cart
{
    public class TotalItem
    {
        [JsonProperty("item_id")]
        public long ItemId { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("base_price")]
        public decimal BasePrice { get; set; }

        [JsonProperty("qty")]
        public decimal Qty { get; set; }

        [JsonProperty("row_total")]
        public decimal RowTotal { get; set; }

        [JsonProperty("base_row_total")]
        public decimal BaseRowTotal { get; set; }

        [JsonProperty("row_total_with_discount")]
        public decimal RowTotalWithDiscount { get; set; }

        [JsonProperty("tax_amount")]
        public decimal TaxAmount { get; set; }

        [JsonProperty("base_tax_amount")]
        public decimal BaseTaxAmount { get; set; }

        [JsonProperty("tax_percent")]
        public long TaxPercent { get; set; }

        [JsonProperty("discount_amount")]
        public decimal DiscountAmount { get; set; }

        [JsonProperty("base_discount_amount")]
        public decimal BaseDiscountAmount { get; set; }

        [JsonProperty("discount_percent")]
        public long DiscountPercent { get; set; }

        [JsonProperty("price_incl_tax")]
        public decimal PriceIncludingTax { get; set; }

        [JsonProperty("base_price_incl_tax")]
        public decimal BasePriceIncludingTax { get; set; }

        [JsonProperty("row_total_incl_tax")]
        public decimal RowTotalIncludingTax { get; set; }

        [JsonProperty("base_row_total_incl_tax")]
        public decimal BaseRowTotalIncludingTax { get; set; }

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
