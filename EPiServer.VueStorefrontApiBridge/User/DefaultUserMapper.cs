using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.User.Invoice;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public class DefaultUserMapper : IUserMapper
    {
        protected readonly IInvoiceAdapter InvoiceAdapter;

        public DefaultUserMapper(IInvoiceAdapter invoiceAdapter)
        {
            InvoiceAdapter = invoiceAdapter;
        }

        public UserModel MapUser(ApplicationUser user)
        {
            if (user == null)
                return null;

            var userContact = CustomerContext.Current.GetContactById(Guid.Parse(user.Id));
            //var userAddresses = userContact.ContactAddresses?.Select(a => MapAddress(userContact, a)).ToList();
            var userAddresses = MapAddresses(userContact);
            
            return new UserModel
            {
                Id = user.Id,
                LastName = userContact?.LastName ?? string.Empty,
                FirstName = userContact?.FirstName ?? string.Empty,
                Email = userContact?.Email ?? string.Empty,

                CreatedAt = user.CreationDate,
                UpdatedAt = user.CreationDate, // TODO can we get this from epi ? 
                Addresses = userAddresses,
                DefaultShippingId = userAddresses?.FirstOrDefault(a => a.DefaultShipping)?.Id,
                DefaultBillingId = userAddresses?.FirstOrDefault(a => a.DefaultBilling)?.Id
            };
        }

        protected List<UserAddressModel> MapAddresses(CustomerContact contact)
        {
            var outAddresses = new List<UserAddressModel>();
            var addresses = contact.ContactAddresses;

            if (addresses == null)
                return outAddresses;

            // Temporary workaround. Vue-Storefront issue #2356
            // Manually assign address.id as the index
            for (var i = 0; i < addresses.Count(); i++)
                outAddresses.Add(MapAddress(contact, i, addresses.ElementAt(i)));

            return outAddresses;
        }

        protected UserAddressModel MapAddress(CustomerContact contact, int index, CustomerAddress address)
        {
            var street = new List<string>(2);

            if (address.Line1 != null)
                street.Add(address.Line1);

            if (address.Line2 != null)
                street.Add(address.Line2);

            var isBillingAddress = (address.AddressType & CustomerAddressTypeEnum.Billing) != 0;
            var isDefaultBillingAddress = contact.PreferredBillingAddress?.AddressId == address.AddressId;
            var isDeffalultShipping = contact.PreferredShippingAddress?.AddressId == address.AddressId;
            var invoiceInformation = isBillingAddress ? InvoiceAdapter.GetInvoiceInformation(contact, address) : null;

            return new UserAddressModel
            {
                // Temporary workaround. Vue-Storefront issue #2356
                Id = index, //address.AddressId.ToString(),
                CustomerId = address.ContactId.ToString(),
                Firstname = address.FirstName,
                Lastname = address.LastName,
                DefaultShipping = isDeffalultShipping,
                DefaultBilling = isDefaultBillingAddress,
                Region = new UserAddressModel.RegionModel
                {
                    Region = address.RegionCode
                },
                City = address.City,
                CountryId = address.CountryCode,
                Postcode = address.PostalCode,
                Telephone = address.DaytimePhoneNumber,
                Street = street,
                Company = invoiceInformation?.Company,
                VatId = invoiceInformation?.VatId
            };
        }
    }
}