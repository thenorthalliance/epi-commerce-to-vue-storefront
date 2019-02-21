using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class CartRequest
    {
        [JsonProperty("cartItem")]
        public CartItem CartItem { get; set; }
    }
}
