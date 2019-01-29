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
        private readonly ApplicationUserManager<TUser> _userManager;

        protected UserAdapter(ApplicationUserManager<TUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserModel> GetUserByCredentials(string userLogin, string userPassword)
        {
            return MapUser(await _userManager.FindAsync(userLogin, userPassword));
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            return MapUser(await _userManager.FindByIdAsync(userId));
        }

        public async Task<UserModel> CreateUser(UserCreateModel newUser)
        {
            var appUser = CreateNewUser(newUser.Customer.Email);
            var result = await _userManager.CreateAsync(appUser, newUser.Password);

            if (!result.Succeeded)
            {
                LogManager.GetLogger(GetType()).Information(
                    $"CreateUser failed: {string.Join(Environment.NewLine, result.Errors)}");

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

            var user = await _userManager.FindByIdAsync(appUser.Id);
            return MapUser(user);
        }

        protected abstract TUser CreateNewUser(string userEmail);

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