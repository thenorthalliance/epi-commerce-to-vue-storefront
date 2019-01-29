using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class UserModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("created_in")]
        public string CreatedIn { get; set; } = "default store";

        [JsonProperty("store_id")]
        public int StoreId { get; set; } = 1;

        [JsonProperty("website_id")]
        public int WebsiteId { get; set; } = 1;

        [JsonProperty("group_id")]
        public int GroupId { get; set; } = 1;

        [JsonProperty("addresses")]
        public List<object> Addresses { get; set; } = new List<object>();

        [JsonProperty("disable_auto_group_change")]
        public int DisableAutoGroupChange { get; set; } = 0;
    }
}