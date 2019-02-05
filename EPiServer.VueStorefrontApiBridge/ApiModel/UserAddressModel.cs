using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class UserAddressModel
    {
        public class RegionModel
        {
            [JsonProperty("region")]
            public string Region { get; set; }
        }

        [JsonProperty("id")]
        //       public string Id { get; set; } 
        public int Id { get; set; } // Temporary workaround. Vue-Storefront issue #2356
        
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }


        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("street")]
        public List<string> Street { get; set; } = new List<string>();

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public RegionModel Region { get; set; } = new RegionModel();

        [JsonProperty("country_id")]
        public string CountryId { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }

        [JsonProperty("default_shipping")]
        public bool DefaultShipping { get; set; }

        [JsonProperty("default_billing")]
        public bool DefaultBilling { get; set; }

        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public string Company { get; set; }

        [JsonProperty("vat_id", NullValueHandling = NullValueHandling.Ignore)]
        public string VatId { get; set; }
    }
}