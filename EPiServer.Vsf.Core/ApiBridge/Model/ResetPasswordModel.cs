using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model
{
    public class ResetPasswordModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}