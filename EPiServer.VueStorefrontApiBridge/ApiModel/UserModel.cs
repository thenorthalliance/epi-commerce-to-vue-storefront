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
        public List<UserAddressModel> Addresses { get; set; } = new List<UserAddressModel>();

        [JsonProperty("disable_auto_group_change")]
        public int DisableAutoGroupChange { get; set; } = 0;


        [JsonProperty("default_shipping", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultShippingId { get; set; }
//        public int? DefaultShippingId { get; set; }  // Temporary workaround. Vue-Storefront issue #2356

        [JsonProperty("default_billing", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultBillingId { get; set; }
//        public int? DefaultBillingId { get; set; }  // Temporary workaround. Vue-Storefront issue #2356
    }
}