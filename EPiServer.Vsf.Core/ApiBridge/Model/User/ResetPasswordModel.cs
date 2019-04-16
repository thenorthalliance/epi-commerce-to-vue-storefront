using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.User
{
    public class ResetPasswordModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}