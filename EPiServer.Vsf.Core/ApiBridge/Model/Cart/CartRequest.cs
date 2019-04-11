using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Cart
{
    public class CartRequest
    {
        [JsonProperty("cartItem")]
        public CartItem CartItem { get; set; }
    }
}
