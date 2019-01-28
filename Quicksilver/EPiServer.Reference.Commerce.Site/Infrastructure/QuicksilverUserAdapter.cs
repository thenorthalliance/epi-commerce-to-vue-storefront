using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.User;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
    [ServiceConfiguration(typeof(IUserAdapter), Lifecycle = ServiceInstanceScope.Transient)]
    public class QuicksilverUserAdapter : IUserAdapter
    {
        private readonly ApplicationUserManager<SiteUser> _userManager;

        public QuicksilverUserAdapter(ApplicationUserManager<SiteUser> userManager)
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

        public async Task<UserModel> CreareUser(UserCreateModel newUser)
        {
            var newUserId = Guid.NewGuid().ToString();

            var result = await _userManager.CreateAsync(new SiteUser
            {
                Id = newUserId,
                Username = newUser.Customer.Email,
                Email = newUser.Customer.Email,
                CreationDate = DateTime.UtcNow,
                Addresses = new List<CustomerAddress>(),
                RegistrationSource = "vuestorefront",
                AcceptMarketingEmail = false,
                Comment = "Created via VueStorefront",
                EmailConfirmed = false,
                IsLockedOut = false,
                IsApproved = true,
            }, newUser.Password);

            if (!result.Succeeded)
            {
                LogManager.GetLogger(GetType()).Information(
                    $"CreateUser failed: {string.Join(Environment.NewLine, result.Errors)}");

                return null;
            }

            var newContact = CustomerContact.CreateInstance();
            newContact.PrimaryKeyId = new PrimaryKeyId(new Guid(newUserId));
            newContact.UserId = "String:" + newUser.Customer.Email; //See UserService.cs:124        
            newContact.AcceptMarketingEmail = false;
            newContact.FirstName = newUser.Customer.Firstname;
            newContact.LastName = newUser.Customer.Lastname;
            newContact.Email = newUser.Customer.Email;
            newContact.SaveChanges();

            var user = await _userManager.FindByIdAsync(newUserId);
            return MapUser(user);
        }

        private UserModel MapUser(SiteUser user)
        {
            if (user == null)
                return null;

            var userContact = CustomerContext.Current.GetContactById(Guid.Parse(user.Id));

            return new UserModel
            {
                Id = user.Id,
                Lastname = userContact?.LastName ?? string.Empty,
                Firstname = userContact?.FirstName ?? string.Empty,
                Email = userContact?.Email ?? string.Empty,

                CreatedAt = user.CreationDate,
                UpdatedAt = user.CreationDate,
            };
        }
    }
}