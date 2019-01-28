using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class UserRefreshTokenModel
    {
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}