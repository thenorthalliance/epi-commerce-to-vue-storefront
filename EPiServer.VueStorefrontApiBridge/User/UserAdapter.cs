using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Logging;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public abstract class UserAdapter<TUser> : IUserAdapter where TUser : ApplicationUser, new()
    {
        protected readonly ApplicationUserManager<TUser> UserManager;

        protected UserAdapter(ApplicationUserManager<TUser> userManager)
        {
            UserManager = userManager;
        }

        public async Task<UserModel> GetUserByCredentials(string userLogin, string userPassword)
        {
            return MapUser(await UserManager.FindAsync(userLogin, userPassword));
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            return MapUser(await UserManager.FindByIdAsync(userId));
        }

        public async Task<UserModel> CreateUser(UserCreateModel newUser)
        {
            var appUser = await CreateNewUser(newUser.Customer.Email);
            var result = await UserManager.CreateAsync(appUser, newUser.Password);

            if (!result.Succeeded)
            {
                LogDebugErrors("CreateUser failed", result.Errors);
                return null;
            }
            
            var newContact = CustomerContact.CreateInstance();
            newContact.PrimaryKeyId = new PrimaryKeyId(new Guid(appUser.Id));
            newContact.UserId = "String:" + newUser.Customer.Email; //See UserService.cs:124        
            newContact.AcceptMarketingEmail = false;
            newContact.FirstName = newUser.Customer.FirstName;
            newContact.LastName = newUser.Customer.LastName;
            newContact.Email = newUser.Customer.Email;
            newContact.SaveChanges();

            var user = await UserManager.FindByIdAsync(appUser.Id);
            return MapUser(user);
        }

        public async Task<bool> UpdateUser(string userId, UserModel updatedUser)
        {
            var user = await UserManager.FindByIdAsync(userId);
            user.UserName = updatedUser.Email;
            user.Email = updatedUser.Email;

            var result = await UserManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                LogDebugErrors("UpdateUser failed", result.Errors);
                return false;
            }
            
            var currentContant = CustomerContext.Current.GetContactById(new Guid(userId));
            currentContant.FirstName = updatedUser.FirstName;
            currentContant.LastName = updatedUser.LastName;
            currentContant.Email = updatedUser.Email;
            currentContant.UserId = "String:" + updatedUser.Email; //See UserService.cs:124  
            currentContant.SaveChanges();

            return true;
        }

        public async Task<bool> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = await UserManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                LogDebugErrors($"Change password for user '{userId}' failed", result.Errors);
                return false;
            }

            return true;
        }

        public abstract Task<bool> SendResetPasswordEmail(string userEmail);

        
        protected abstract Task<TUser> CreateNewUser(string userEmail);
        
        protected void LogDebugErrors(string message, IEnumerable<string> errors = null)
        {
            var errorMessage = errors != null ? 
                    $"{message}:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}" :
                    message;

            LogManager.GetLogger(GetType()).Debug(errorMessage);
        }

        private UserModel MapUser(ApplicationUser user)
        {
            if (user == null)
                return null;

            var userContact = CustomerContext.Current.GetContactById(Guid.Parse(user.Id));
            //var userAddresses = userContact.ContactAddresses?.Select(a => MapAddress(userContact, a)).ToList();
            var userAddresses = MapAddresses(userContact, userContact.ContactAddresses?.ToList());

            return new UserModel
            {
                Id = user.Id,
                LastName = userContact?.LastName ?? string.Empty,
                FirstName = userContact?.FirstName ?? string.Empty,
                Email = userContact?.Email ?? string.Empty,

                CreatedAt = user.CreationDate,
                UpdatedAt = user.CreationDate,
                Addresses = userAddresses,
                DefaultShippingId = userAddresses?.FirstOrDefault(a => a.DefaultShipping)?.Id,
                DefaultBillingId = userAddresses?.FirstOrDefault(a => a.DefaultBilling)?.Id
            };
        }

        private List<UserAddressModel> MapAddresses(CustomerContact contact, List<CustomerAddress> addresses)
        {
            var outAddresses = new List<UserAddressModel>();

            if (addresses == null)
                return outAddresses;

            // Temporary workaround. Vue-Storefront issue #2356
            // Manually assign address.id as the index
            for (var i = 0; i < addresses.Count(); i++)
                outAddresses.Add(MapAddress(contact, i, addresses.ElementAt(i)));

            return outAddresses;
        }

        private UserAddressModel MapAddress(CustomerContact contact, int index, CustomerAddress address)
        {

            var street = new List<string>(2);

            if(address.Line1 != null)
                street.Add(address.Line1);

            if (address.Line2 != null)
                street.Add(address.Line2);

            var isBillingAddress = (address.AddressType & CustomerAddressTypeEnum.Billing) != 0;
            var isDefaultBillingAddress = contact.PreferredBillingAddress?.AddressId == address.AddressId;
            var isDeffalultShipping = contact.PreferredShippingAddress?.AddressId == address.AddressId;

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
                Company = isBillingAddress ? "ACME!" : null,
                VatId = isBillingAddress ? "VAT123" : null
            };
        }
    }
}