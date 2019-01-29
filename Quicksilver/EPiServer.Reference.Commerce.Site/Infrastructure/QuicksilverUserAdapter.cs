using System;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.User;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
    [ServiceConfiguration(typeof(IUserAdapter), Lifecycle = ServiceInstanceScope.Transient)]
    public class QuickSilverUserAdapter : UserAdapter<SiteUser>
    {
        public QuickSilverUserAdapter(ApplicationUserManager<SiteUser> userManager) : base(userManager)
        {}

        protected override SiteUser CreateNewUser(string userEmail)
        {
            return new SiteUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = userEmail,
                Email = userEmail,
                EmailConfirmed = false,
                IsLockedOut = false,
                IsApproved = true,
                CreationDate = DateTime.UtcNow
            };
        }
    }
}