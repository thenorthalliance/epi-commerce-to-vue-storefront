using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        public List<OrderShippingAssignments> ShippingAssignments { get; set; }
    }
}