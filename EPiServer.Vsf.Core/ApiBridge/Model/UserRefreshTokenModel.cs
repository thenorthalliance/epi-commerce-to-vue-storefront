using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model
{
    public class UserRefreshTokenModel
    {
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}