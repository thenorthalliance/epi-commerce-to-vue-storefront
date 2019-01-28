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
                Username = newUser.customer.email,
                Email = newUser.customer.email,
                CreationDate = DateTime.UtcNow,
                Addresses = new List<CustomerAddress>(),
                RegistrationSource = "vuestorefront",
                AcceptMarketingEmail = false,
                Comment = "Created via VueStorefront",
                EmailConfirmed = false,
                IsLockedOut = false,
                IsApproved = true,
            }, newUser.password);

            if (!result.Succeeded)
            {
                LogManager.GetLogger(GetType()).Information(
                    $"CreateUser failed: {string.Join(Environment.NewLine, result.Errors)}");

                return null;
            }

            var newContact = CustomerContact.CreateInstance();
            newContact.PrimaryKeyId = new PrimaryKeyId(new Guid(newUserId));
            newContact.UserId = "String:" + newUser.customer.email; //See UserService.cs:124
            newContact.AcceptMarketingEmail = false;
            newContact.FirstName = newUser.customer.firstname;
            newContact.LastName = newUser.customer.lastname;
            newContact.Email = newUser.customer.email;
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
                id = user.Id,
                lastname = userContact?.LastName ?? string.Empty,
                firstname = userContact?.FirstName ?? string.Empty,
                email = userContact?.Email ?? string.Empty,

                created_at = user.CreationDate,
                updated_at = user.CreationDate,
            };
        }
    }
}