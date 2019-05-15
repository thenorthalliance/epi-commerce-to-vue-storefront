using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderAddress
    {
        [JsonProperty("address_type")]
        public string AddressType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("country_id")]
        public string CountryId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("entity_id")]
        public string EntityId { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("parent_id")]
        public int ParentOrderId { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("street")]
        public List<string> Street { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }
    }
}