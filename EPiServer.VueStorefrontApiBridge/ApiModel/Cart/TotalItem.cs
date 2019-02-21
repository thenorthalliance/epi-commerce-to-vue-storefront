using EPiServer.Commerce.Order;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class TotalItem
    {
        public TotalItem()
        {
        }

        public TotalItem(ILineItem item)
        {
            ItemId = item.LineItemId;
            Price = (long) item.PlacedPrice;
            BasePrice = (long) item.PlacedPrice;
            Qty = (long) item.Quantity;
            RowTotal = 0;
            BaseRowTotal = 0;
            RowTotalWithDiscount = 0;
            TaxAmount = 0;
            BaseTaxAmount = 0;
            TaxPercent = 0;
            DiscountAmount = 0;
            BaseDiscountAmount = 0;
            DiscountPercent = 0;
            Options = "";
            WeeeTaxAppliedAmount = null;
            WeeeTaxApplied = null;
            Name = item.DisplayName;
            ProductOption = new ProductOption(); //TODO get products options
        }

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
