using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Requests
{
    public class PayPalTransactionDetails
    {
        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }
    }
}