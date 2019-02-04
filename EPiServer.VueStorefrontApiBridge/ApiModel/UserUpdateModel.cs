using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class UserUpdateModel
    {
        [JsonProperty("customer")]
        public UserModel Customer { get; set; }
    }
}