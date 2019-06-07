using System;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Requests
{
    public class PayPalCreateOrderRequest
    {
        [JsonProperty("cartId")]
        public Guid CartId { get; set; }

        [JsonProperty("transaction")]
        public PayPalTransaction Transaction { get; set; }
    }
}