using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
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
        public long Amount { get; set; }

        [JsonProperty("base_amount")]
        public long BaseAmount { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("price_excl_tax")]
        public long PriceExclTax { get; set; }

        [JsonProperty("price_incl_tax")]
        public long PriceInclTax { get; set; }
    }
}
