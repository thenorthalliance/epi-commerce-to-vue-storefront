using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.User
{
    public class UserLoginModel
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}