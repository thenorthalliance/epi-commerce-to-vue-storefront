using EPiServer.Commerce.Order;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class CartItem
    {
        public CartItem()
        {
        }
        public CartItem(ILineItem item, string cartId)
        {
            ItemId = item.LineItemId;
            Sku = item.Code;
            Qty = (int) item.Quantity;
            Name = item.DisplayName;
            Price = (int) item.PlacedPrice;
            ProductType = "configurable";
            QuoteId = cartId;
        }

        [JsonProperty("item_id")]
        public int? ItemId { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("qty")]
        public int Qty { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("quote_id")]
        public string QuoteId { get; set; }

        [JsonProperty("product_option")]
        public ProductOption ProductOption { get; set; }
    }
}
