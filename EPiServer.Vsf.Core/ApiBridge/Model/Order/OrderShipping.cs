using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderShipping
    {
        [JsonProperty("address")]
        public OrderAddress Address { get; set; }
    }
}