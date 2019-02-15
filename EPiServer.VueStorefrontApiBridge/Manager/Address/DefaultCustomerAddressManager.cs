using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.Manager.Address
{
    public class DefaultCustomerAddressManager : ICustomerAddressManager
    {
        public bool UpdateContactAddresses(string userId, CustomerContact currentContact, IEnumerable<UserAddressModel> userAddresses)
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

        public CustomerAddress CreateCustomerAddress(UserAddressModel addressModel, Guid userGuid)
        {
            var customerAddress = CustomerAddress.CreateInstance();
            UpdateContactAddressData(customerAddress, addressModel);
            customerAddress.ContactId = new PrimaryKeyId(userGuid);
            customerAddress.AddressId = new PrimaryKeyId(Guid.NewGuid());

            return customerAddress;
        }

        public void UpdateContactAddressData(CustomerAddress customerAddress, UserAddressModel updatedAddressModel)
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
