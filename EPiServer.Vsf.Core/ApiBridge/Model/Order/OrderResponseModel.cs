using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderResponseModel
    {
        [JsonProperty("backendOrderId")]
        public int OrderId { get; set; }
    }
}