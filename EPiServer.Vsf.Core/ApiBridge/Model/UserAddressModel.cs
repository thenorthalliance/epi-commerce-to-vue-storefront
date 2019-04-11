using System.Collections.Generic;
using Mediachase.Commerce.Customers;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model
{
    public class UserAddressModel
    {
        public UserAddressModel()
        {
        }

        public UserAddressModel(CustomerAddress address, InvoiceInformation invoiceInformation, bool isDefaultBilling, bool isDefaultShipping)
        {
            var street = new List<string>(2);
            if (address.Line1 != null)
                street.Add(address.Line1);

            if (address.Line2 != null)
                street.Add(address.Line2);

            Id = address.AddressId.ToString();
            CustomerId = address.ContactId.ToString();
            Firstname = address.FirstName;
            Lastname = address.LastName;
            DefaultShipping = isDefaultShipping;
            DefaultBilling = isDefaultBilling;
            Region = new RegionModel
            {
                Region = address.RegionName
            };
            City = address.City;
            CountryId = address.CountryCode;
            Postcode = address.PostalCode;
            Telephone = address.DaytimePhoneNumber;
            Street = street;
            Company = invoiceInformation?.Company;
            VatId = invoiceInformation?.VatId;
        }
        public class RegionModel
        {
            [JsonProperty("region")]
            public string Region { get; set; }
        }

        [JsonProperty("id")]
        public string Id { get; set; } 
        
        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("street")]
        public List<string> Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public RegionModel Region { get; set; }

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