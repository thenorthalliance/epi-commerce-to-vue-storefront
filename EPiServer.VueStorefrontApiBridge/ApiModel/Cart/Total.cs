using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class Total
    {
        public Total()
        {
        }
        public Total(ICart cart)
        {
            var items = cart.GetAllLineItems().ToList();
            GrandTotal = (long) cart.GetTotal().Amount;
            WeeeTaxAppliedAmount = null;
            BaseCurrencyCode = cart.Currency.CurrencyCode;
            QuoteCurrencyCode = cart.Currency.CurrencyCode;
            ItemsQty = items.Count();
            Items = items.Select(itm => new TotalItem(itm)).ToList();
            TotalSegments = CreateSegments(cart);
        }
        [JsonProperty("grand_total")]
        public long GrandTotal { get; set; }

        [JsonProperty("weee_tax_applied_amount")]
        public string WeeeTaxAppliedAmount { get; set; }

        [JsonProperty("base_currency_code")]
        public string BaseCurrencyCode { get; set; }

        [JsonProperty("quote_currency_code")]
        public string QuoteCurrencyCode { get; set; }

        [JsonProperty("items_qty")]
        public long ItemsQty { get; set; }

        [JsonProperty("items")]
        public List<TotalItem> Items { get; set; }

        [JsonProperty("total_segments")]
        public List<TotalSegment> TotalSegments { get; set; }

        //TODO FILL IT OR CHANGE
        private static List<TotalSegment> CreateSegments(ICart cart)
        {
            var result = new List<TotalSegment>();
            var subTotal = cart.GetSubTotal();
            result.Add(new TotalSegment
            {
                Code = "subtotal",
                Title = "Subtotal",
                Value = (long?) subTotal.Amount
            });

            var shippingTotal = cart.GetShippingTotal();
            result.Add(new TotalSegment
            {
                Code = "shipping",
                Title = "Shipping",
                Value = (long?)shippingTotal.Amount
            });
            result.Add(new TotalSegment
            {
                Code = "handling",
                Title = "Handling",
                Value = (long?)shippingTotal.Amount
            });
            var grandTotal = cart.GetTotal();
            result.Add(new TotalSegment
            {
                Code = "grand_total",
                Title = "Grant Total",
                Value = (long?)grandTotal.Amount
            });
            return result;
        }
    }
}
