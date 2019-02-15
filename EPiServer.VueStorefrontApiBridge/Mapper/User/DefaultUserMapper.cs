using System;
using System.Linq;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.VueStorefrontApiBridge.Adapter.Invoice;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.Mapper.User
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
            var userAddresses = userContact?.ContactAddresses?.Select(addr => MapAddress(userContact, addr)).ToList();
            
            return new UserModel
            {
                Id = user.Id,
                LastName = userContact?.LastName ?? string.Empty,
                FirstName = userContact?.FirstName ?? string.Empty,
                Email = userContact?.Email ?? string.Empty,
                CreatedAt = user.CreationDate,
                UpdatedAt = userContact?.Modified ?? user.CreationDate,
                Addresses = userAddresses,
                DefaultShippingId = userAddresses?.FirstOrDefault(a => a.DefaultShipping)?.Id,
                DefaultBillingId = userAddresses?.FirstOrDefault(a => a.DefaultBilling)?.Id
            };
        }

        protected UserAddressModel MapAddress(CustomerContact contact, CustomerAddress address)
        {
            var isBillingAddress = (address.AddressType & CustomerAddressTypeEnum.Billing) != 0;
            var isDefaultBillingAddress = contact.PreferredBillingAddress?.AddressId == address.AddressId;
            var isDefaultShipping = contact.PreferredShippingAddress?.AddressId == address.AddressId;
            var invoiceInformation = isBillingAddress ? InvoiceAdapter.GetInvoiceInformation(contact, address) : null;

            return new UserAddressModel(address, invoiceInformation, isDefaultBillingAddress, isDefaultShipping);
        }
    }
}