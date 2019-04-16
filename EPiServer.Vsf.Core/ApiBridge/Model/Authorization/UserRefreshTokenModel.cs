using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Authorization
{
    public class UserRefreshTokenModel
    {
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}