using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderPayment
    {
        [JsonProperty("additional_information")]
        public List<string> AdditionalInformation { get; set; } //first index is the payment method name shown in my orders... (see UserSingleOrder.ts)
        public string account_status { get; set; }
        public float amount_authorized { get; set; }
        public float amount_ordered { get; set; }
        public float base_amount_authorized { get; set; }
        public float base_amount_ordered { get; set; }
        public int base_shipping_amount { get; set; }
        public object cc_last4 { get; set; }
        public int entity_id { get; set; }
        public string method { get; set; }
        public int parent_id { get; set; }
        public int shipping_amount { get; set; }
    }
}
