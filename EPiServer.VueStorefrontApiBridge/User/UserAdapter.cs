using System;
using System.Collections.Generic;
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
            
            return new UserModel
            {
                Id = user.Id,
                LastName = userContact?.LastName ?? string.Empty,
                FirstName = userContact?.FirstName ?? string.Empty,
                Email = userContact?.Email ?? string.Empty,

                CreatedAt = user.CreationDate,
                UpdatedAt = user.CreationDate,
                Addresses = new List<object>()
            };
        }
    }
}