using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.User
{
    public class UserUpdateModel
    {
        [JsonProperty("customer")]
        public VsfUser Customer { get; set; }
    }
}