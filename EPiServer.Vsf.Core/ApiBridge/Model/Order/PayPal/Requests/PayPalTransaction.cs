using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Requests
{
    public class PayPalTransaction
    {
        [JsonProperty("amount")]
        public PayPalTransactionDetails Amount { get; set; }
    }
}