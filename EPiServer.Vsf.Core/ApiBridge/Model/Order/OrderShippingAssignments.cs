using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderShippingAssignments
    {
        [JsonProperty("shipping")]
        public OrderShipping Shipping { get; set; }
    }
}