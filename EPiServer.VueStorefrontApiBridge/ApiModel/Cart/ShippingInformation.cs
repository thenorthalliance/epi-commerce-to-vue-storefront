using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class ShippingInformation
    {
        [JsonProperty("payment_methods")]
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        [JsonProperty("totals")]
        public Total Totals { get; set; }
    }
}
