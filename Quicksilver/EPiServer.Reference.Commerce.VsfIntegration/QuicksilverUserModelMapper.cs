using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.Vsf.Core.ApiBridge.Model;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.VsfIntegration
{
    public static class QuicksilverUserModelMapper
    {
        public static UserModel MapToVsfModel(SiteUser user)
        {
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

        public static UserAddressModel MapAddress(CustomerContact contact, CustomerAddress address)
        {
            var isBillingAddress = (address.AddressType & CustomerAddressTypeEnum.Billing) != 0;
            var isDefaultBillingAddress = contact.PreferredBillingAddress?.AddressId == address.AddressId;
            var isDefaultShipping = contact.PreferredShippingAddress?.AddressId == address.AddressId;
            var invoiceInformation = isBillingAddress ? CreateMockedInvoice(contact, address) : null;

            return new UserAddressModel(address, invoiceInformation, isDefaultBillingAddress, isDefaultShipping);
        }

        private static InvoiceInformation CreateMockedInvoice(CustomerContact customerContact, CustomerAddress address)
        {
            return new InvoiceInformation
            {
                VatId = "VAT123",
                Company = "ACME"
            };
        }

        public static bool UpdateCustomerContact(QuickSilverUserAdapter quickSilverUserAdapter, string userId, UserModel updatedUser)
        {
            var currentContact = CustomerContext.Current.GetContactById(new Guid(userId));
            currentContact.FullName = string.Concat(updatedUser.FirstName, " ", updatedUser.LastName);
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
                {
                    currentContact.PreferredBillingAddress = address;

                }

                if (updatedAddress.DefaultShipping)
                {
                    currentContact.PreferredShippingAddress = address;
                }
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
            {
                customerAddress.AddressType |= CustomerAddressTypeEnum.Billing;
            }

            if (updatedAddressModel.DefaultShipping)
            {
                customerAddress.AddressType |= CustomerAddressTypeEnum.Shipping;
            }
        }
    }
}