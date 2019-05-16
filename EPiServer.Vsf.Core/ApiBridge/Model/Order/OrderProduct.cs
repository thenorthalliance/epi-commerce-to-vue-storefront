using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderProduct
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("qty")]
        public int Quantity { get; set; }
    }
}
