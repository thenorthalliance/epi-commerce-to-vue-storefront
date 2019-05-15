using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class Product
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("qty")]
        public int Quantity { get; set; }
    }
}
