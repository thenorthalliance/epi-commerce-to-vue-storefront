using System;
using System.Collections.Generic;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.Manager.Address
{
    public interface ICustomerAddressManager
    {
        bool UpdateContactAddresses(string userId, CustomerContact currentContact,
            IEnumerable<UserAddressModel> userAddresses);

        CustomerAddress CreateCustomerAddress(UserAddressModel addressModel, Guid userGuid);
        void UpdateContactAddressData(CustomerAddress customerAddress, UserAddressModel updatedAddressModel);
    }
}
