using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderProductDetails
    {
        [JsonProperty("row_total_incl_tax")]
        public decimal RowTotalIncludingTax { get; set; }

        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("price_incl_tax")]
        public decimal PriceIncludingTax { get; set; }

        [JsonProperty("order_id")]
        public int OrderId { get; set; }

        [JsonProperty("qty_ordered")]
        public decimal QuantityOrdered { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        //public int amount_refunded { get; set; }
        //public string applied_rule_ids { get; set; }
        //public int base_amount_refunded { get; set; }
        //public float base_discount_amount { get; set; }
        //public int base_discount_invoiced { get; set; }
        //public int base_discount_tax_compensation_amount { get; set; }
        //public decimal base_original_price { get; set; }
        //public decimal base_price { get; set; }
        //public decimal base_price_incl_tax { get; set; }
        //public int base_row_invoiced { get; set; }
        //public decimal base_row_total { get; set; }
        //public decimal base_row_total_incl_tax { get; set; }
        //public float base_tax_amount { get; set; }
        //public int base_tax_invoiced { get; set; }qt
        //public string created_at { get; set; }
        //public float discount_amount { get; set; }
        //public int discount_invoiced { get; set; }
        //public int discount_percent { get; set; }
        //public int free_shipping { get; set; }
        //public int discount_tax_compensation_amount { get; set; }
        //public int is_qty_decimal { get; set; }
        //public int is_virtual { get; set; }
        //public int no_discount { get; set; }

        //public int original_price { get; set; }

        //public int product_id { get; set; }
        //public string product_type { get; set; }
        //public int qty_canceled { get; set; }
        //public int qty_invoiced { get; set; }

        //public int qty_refunded { get; set; }
        //public int qty_shipped { get; set; }
        //public int quote_item_id { get; set; }
        //public int row_invoiced { get; set; }
        //public int row_total { get; set; }

        //public int row_weight { get; set; }
        //public int store_id { get; set; }
        //public float tax_amount { get; set; }
        //public int tax_invoiced { get; set; }
        //public int tax_percent { get; set; }
        //public string updated_at { get; set; }
        //public int weight { get; set; }
    }
}
