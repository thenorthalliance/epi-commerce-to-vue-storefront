using System;
using System.Threading.Tasks;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.User;
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