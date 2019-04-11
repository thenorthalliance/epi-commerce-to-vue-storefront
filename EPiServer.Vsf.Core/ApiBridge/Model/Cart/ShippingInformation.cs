using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Cart
{
    public class ShippingInformation
    {
        [JsonProperty("payment_methods")]
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        [JsonProperty("totals")]
        public Total Totals { get; set; }
    }
}
