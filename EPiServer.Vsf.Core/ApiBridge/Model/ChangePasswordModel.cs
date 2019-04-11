using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model
{
    public class ChangePasswordModel
    {
        [JsonProperty("currentPassword")]
        public string CurrentPassword { get; set; }

        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
    }
}