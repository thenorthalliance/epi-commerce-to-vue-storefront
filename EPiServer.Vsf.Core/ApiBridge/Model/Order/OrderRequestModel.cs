using System;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderRequestModel
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("cart_id")]
        public Guid CartId { get; set; }

        [JsonProperty("products")]
        public OrderProduct[] products { get; set; }

        [JsonProperty("addressInformation")]
        public OrderAddressInformation AddressInformation { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("transmited")]
        public bool Transmited { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
