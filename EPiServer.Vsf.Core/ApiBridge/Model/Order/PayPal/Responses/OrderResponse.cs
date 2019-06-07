using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Responses
{
    public class PayPalOrderResponse
    {
        [JsonProperty("id")]
        public string OrderId { get; set; } 
    }
}
