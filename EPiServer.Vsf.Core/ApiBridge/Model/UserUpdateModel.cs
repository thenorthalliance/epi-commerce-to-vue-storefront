using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model
{
    public class UserUpdateModel
    {
        [JsonProperty("customer")]
        public UserModel Customer { get; set; }
    }
}