using System;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.Manager.Address;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.Manager.Contact
{
    public class DefaultCustomerContactManager : ICustomerContactManager
    {
        private readonly ICustomerAddressManager _customerAddressManager;

        public DefaultCustomerContactManager(ICustomerAddressManager customerAddressManager)
        {
            _customerAddressManager = customerAddressManager;
        }
        public bool UpdateCustomerContact(string userId, UserModel updatedUser)
        {
            var currentContact = CustomerContext.Current.GetContactById(new Guid(userId));
            currentContact.FullName = string.Concat(updatedUser.FirstName, " ", updatedUser.LastName);
            currentContact.FirstName = updatedUser.FirstName;
            currentContact.LastName = updatedUser.LastName;
            currentContact.Email = updatedUser.Email;
            currentContact.UserId = "String:" + updatedUser.Email; //See UserService.cs:124

            return _customerAddressManager.UpdateContactAddresses(userId, currentContact, updatedUser.Addresses);
        }

    }
}
