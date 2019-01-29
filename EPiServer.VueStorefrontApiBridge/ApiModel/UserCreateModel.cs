using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class UserCreateModel
    {
        [JsonProperty("customer")]
        public CustomerModel Customer { get; set; }
    
        [JsonProperty("password")]
        public string Password { get; set; }
    
        public class CustomerModel
        {
            [JsonProperty("email")]
            public string Email { get; set; }
    
            [JsonProperty("firstname")]
            public string FirstName { get; set; }
    
            [JsonProperty("lastname")]
            public string LastName { get; set; }
        }
    }
}