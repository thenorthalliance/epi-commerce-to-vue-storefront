using System;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Requests
{
    public class PayPalCaptureRequest
    {
        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }

        [JsonProperty("payerId")]
        public string PayerId { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("cartId")]
        public Guid CartId { get; set; }
    }
}