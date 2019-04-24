using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Cart
{
    public class ShippingMethod
    {

        [JsonProperty("carrier_code")]
        public string CarrierCode { get; set; }

        [JsonProperty("method_code")]
        public string MethodCode { get; set; }

        [JsonProperty("carrier_title")]
        public string CarrierTitle { get; set; }

        [JsonProperty("method_title")]
        public string MethodTitle { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("base_amount")]
        public decimal BaseAmount { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("price_excl_tax")]
        public decimal PriceExclTax { get; set; }

        [JsonProperty("price_incl_tax")]
        public decimal PriceInclTax { get; set; }
    }
}
