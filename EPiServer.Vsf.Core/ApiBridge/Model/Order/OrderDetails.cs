using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderDetails
    {
        [JsonProperty("entity_id")]
        public int OrderNumber { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("customer_firstname")]
        public string CustomerFirstname { get; set; }

        [JsonProperty("customer_lastname")]
        public string CustomerLastname { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("total_due")]
        public decimal TotalDue { get; set; }

        [JsonProperty("grand_total")]
        public decimal GrandTotal { get; set; }

        [JsonProperty("items")] //Ordered Products
        public List<OrderProductDetails> OrderedProducts { get; set; }

        [JsonProperty("billing_address")]
        public OrderAddress BillingAddress { get; set; }

        [JsonProperty("payment")]
        public OrderPayment Payment { get; set; }

        //public object[] status_histories { get; set; }

        [JsonProperty("extension_attributes")] // Vue storefront reads shipping address from here...
        public OrderExtensionAttributes ExtensionAttributes { get; set; }

        [JsonProperty("shipping_amount")]
        public decimal ShippingAmount { get; set; }

        [JsonProperty("shipping_description")]
        public string ShippingDescription { get; set; }

        [JsonProperty("shipping_discount_amount")]
        public decimal ShippingDiscountAmount { get; set; }

        [JsonProperty("shipping_discount_tax_compensation_amount")]
        public decimal shippingDiscountTaxCompensationAmount { get; set; }

        [JsonProperty("shipping_incl_tax")]
        public decimal ShippingIncludingTax { get; set; }

        [JsonProperty("shipping_tax_amount")]
        public decimal ShippingTaxAmount { get; set; }

        [JsonProperty("discount_tax_compensation_amount")]
        public decimal DiscountTaxCompensationAmount { get; set; }

        [JsonProperty("tax_amount")]
        public decimal TaxAmount { get; set; }

        [JsonProperty("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonProperty("discount_amount")]
        public decimal DiscountAmount { get; set; }

        //IT IS A HUGE OBJECT, POSSIBLE FIELDS BELOW
        //public decimal total_due { get; set; }su
        //public string applied_rule_ids { get; set; }
        //public string base_currency_code { get; set; }
        //public float base_discount_amount { get; set; }
        //public int base_grand_total { get; set; }
        //public int base_discount_tax_compensation_amount { get; set; }
        //public int base_shipping_amount { get; set; }
        //public int base_shipping_discount_amount { get; set; }
        //public int base_shipping_incl_tax { get; set; }
        //public int base_shipping_tax_amount { get; set; }
        //public int base_subtotal { get; set; }
        //public float base_subtotal_incl_tax { get; set; }
        //public float base_tax_amount { get; set; }
        //public int base_total_due { get; set; }
        //public int base_to_global_rate { get; set; }
        //public int base_to_order_rate { get; set; }
        //public int billing_address_id { get; set; }ta
        //public string customer_email { get; set; }
        //public int customer_group_id { get; set; }
        //public int customer_is_guest { get; set; }
        //public int customer_note_notify { get; set; }

        //public int email_sent { get; set; }
        //public string global_currency_code { get; set; }

        //public string increment_id { get; set; }
        //public int is_virtual { get; set; }
        //public string order_currency_code { get; set; }
        //public string protect_code { get; set; }
        //public int quote_id { get; set; }
        //public string state { get; set; }
        //public string store_currency_code { get; set; }
        //public int store_id { get; set; }
        //public string store_name { get; set; }
        //public int store_to_base_rate { get; set; }
        //public int store_to_order_rate { get; set; }
        //public float subtotal_incl_tax { get; set; }

        //public int total_item_count { get; set; }
        //public int total_qty_ordered { get; set; }
        //public string updated_at { get; set; }
        //public int weight { get; set; }

    }
}
