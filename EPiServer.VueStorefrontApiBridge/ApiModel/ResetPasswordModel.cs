using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class ResetPasswordModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}