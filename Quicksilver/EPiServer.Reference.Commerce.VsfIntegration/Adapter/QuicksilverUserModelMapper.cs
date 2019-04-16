using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.VsfIntegration.Adapter
{
    public static class QuicksilverUserModelMapper
    {
        public static VsfUser MapToVsfModel(SiteUser user)
        {
            var userContact = CustomerContext.Current.GetContactById(Guid.Parse(user.Id));
            var userAddresses = userContact?.ContactAddresses?.Select(addr => MapAddress(userContact, addr)).ToList();

            return new VsfUser
            {
                Id = user.Id,
                LastName = userContact?.LastName ?? String.Empty,
                FirstName = userContact?.FirstName ?? String.Empty,
                Email = userContact?.Email ?? String.Empty,
                CreatedAt = user.CreationDate,
                UpdatedAt = userContact?.Modified ?? user.CreationDate,
                Addresses = userAddresses,
                DefaultShippingId = userAddresses?.FirstOrDefault(a => a.DefaultShipping)?.Id,
                DefaultBillingId = userAddresses?.FirstOrDefault(a => a.DefaultBilling)?.Id
            };
        }

        public static UserAddressModel MapAddress(CustomerContact contact, CustomerAddress address)
        {
            var isBillingAddress = (address.AddressType & CustomerAddressTypeEnum.Billing) != 0;
            var isDefaultBillingAddress = contact.PreferredBillingAddress?.AddressId == address.AddressId;
            var isDefaultShipping = contact.PreferredShippingAddress?.AddressId == address.AddressId;
            var invoiceInformation = isBillingAddress ? CreateMockedInvoice(contact, address) : null;

            return CreateUserAddress(address, invoiceInformation, isDefaultBillingAddress, isDefaultShipping);
        }

        private static InvoiceInformation CreateMockedInvoice(CustomerContact customerContact, CustomerAddress address)
        {
            return new InvoiceInformation
            {
                VatId = "VAT123",
                Company = "ACME"
            };
        }

        public static bool UpdateCustomerContact(QuickSilverUserAdapter quickSilverUserAdapter, string userId, VsfUser updatedUser)
        {
            var currentContact = CustomerContext.Current.GetContactById(new Guid(userId));
            currentContact.FullName = String.Concat(updatedUser.FirstName, " ", updatedUser.LastName);
            currentContact.FirstName = updatedUser.FirstName;
            currentContact.LastName = updatedUser.LastName;
            currentContact.Email = updatedUser.Email;
            currentContact.UserId = "String:" + updatedUser.Email; //See UserService.cs:124

            return UpdateContactAddresses(userId, currentContact, updatedUser.Addresses);
        }

        public static bool UpdateContactAddresses(string userId, CustomerContact currentContact, IEnumerable<UserAddressModel> userAddresses)
        {
            var updatedAddresses = userAddresses?.Where(x => x.DefaultShipping || x.DefaultBilling);
            if (updatedAddresses == null)
            {
                currentContact.SaveChanges();
                return true;
            }

            foreach (var updatedAddress in updatedAddresses)
            {
                var currentAddress = currentContact.ContactAddresses
                    .FirstOrDefault(x => x.AddressId.ToString() == updatedAddress.Id);
                CustomerAddress address;
                if (currentAddress == null)
                {
                    address = CreateCustomerAddress(updatedAddress, new Guid(userId));
                    currentContact.AddContactAddress(address);
                }
                else
                {
                    UpdateContactAddressData(currentAddress, updatedAddress);
                    address = currentAddress;
                    currentContact.UpdateContactAddress(address);
                }

                if (updatedAddress.DefaultBilling)
                    currentContact.PreferredBillingAddress = address;

                if (updatedAddress.DefaultShipping)
                    currentContact.PreferredShippingAddress = address;
            }

            currentContact.SaveChanges();
            return true;
        }

        public static CustomerAddress CreateCustomerAddress(UserAddressModel addressModel, Guid userGuid)
        {
            var customerAddress = CustomerAddress.CreateInstance();
            UpdateContactAddressData(customerAddress, addressModel);
            customerAddress.ContactId = new PrimaryKeyId(userGuid);
            customerAddress.AddressId = new PrimaryKeyId(Guid.NewGuid());

            return customerAddress;
        }

        public static void UpdateContactAddressData(CustomerAddress customerAddress, UserAddressModel updatedAddressModel)
        {
            customerAddress.City = updatedAddressModel.City;
            customerAddress.RegionName = updatedAddressModel.Region?.Region;
            customerAddress.FirstName = updatedAddressModel.Firstname;
            customerAddress.LastName = updatedAddressModel.Lastname;
            customerAddress.Line1 = updatedAddressModel.Street?[0];
            customerAddress.Line2 = updatedAddressModel.Street?[1];
            customerAddress.CountryCode = updatedAddressModel.CountryId;
            customerAddress.PostalCode = updatedAddressModel.Postcode;
            customerAddress.DaytimePhoneNumber = updatedAddressModel.Telephone;

            if (updatedAddressModel.DefaultBilling)
                customerAddress.AddressType |= CustomerAddressTypeEnum.Billing;

            if (updatedAddressModel.DefaultShipping)
                customerAddress.AddressType |= CustomerAddressTypeEnum.Shipping;
        }

        private static UserAddressModel CreateUserAddress(CustomerAddress address, InvoiceInformation invoiceInformation, bool isDefaultBilling, bool isDefaultShipping)
        {
            var userAddressModel = new UserAddressModel();

            var street = new List<string>(2);
            if (address.Line1 != null)
                street.Add(address.Line1);

            if (address.Line2 != null)
                street.Add(address.Line2);

            userAddressModel.Id = address.AddressId.ToString();
            userAddressModel.CustomerId = address.ContactId.ToString();
            userAddressModel.Firstname = address.FirstName;
            userAddressModel.Lastname = address.LastName;
            userAddressModel.DefaultShipping = isDefaultShipping;
            userAddressModel.DefaultBilling = isDefaultBilling;
            userAddressModel.Region = new UserAddressModel.RegionModel
            {
                Region = address.RegionName
            };
            userAddressModel.City = address.City;
            userAddressModel.CountryId = address.CountryCode;
            userAddressModel.Postcode = address.PostalCode;
            userAddressModel.Telephone = address.DaytimePhoneNumber;
            userAddressModel.Street = street;
            userAddressModel.Company = invoiceInformation?.Company;
            userAddressModel.VatId = invoiceInformation?.VatId;

            return userAddressModel;
        }
    }
}