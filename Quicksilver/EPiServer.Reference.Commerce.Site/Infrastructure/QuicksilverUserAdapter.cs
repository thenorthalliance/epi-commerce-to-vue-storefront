using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.Reference.Commerce.Shared.Services;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.User;
using Microsoft.AspNet.Identity;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
    [ServiceConfiguration(typeof(IUserAdapter), Lifecycle = ServiceInstanceScope.Transient)]
    public class QuickSilverUserAdapter : UserAdapter<SiteUser>
    {
        private readonly MailService _mailService;
        private readonly UrlHelper _urlHelper;

        public QuickSilverUserAdapter(ApplicationUserManager<SiteUser> userManager, MailService mailService, UrlHelper urlHelper) : base(userManager)
        {
            _mailService = mailService;
            _urlHelper = urlHelper;
        }

        protected override Task<SiteUser> CreateNewUser(string userEmail)
        {
            return Task.FromResult(new SiteUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = userEmail,
                Email = userEmail,
                EmailConfirmed = false,
                IsLockedOut = false,
                IsApproved = true,
                CreationDate = DateTime.UtcNow
            });
        }

        public override async Task<bool> SendResetPasswordEmail(string userEmail)
        {
            //TEST RESETPASWORD EMAIL SENDING
            var user = await UserManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                LogDebugErrors($"SendResetPasswordEmail: User '{userEmail}' not found");
                return false;
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var passwordResetUrl = _urlHelper.Action("ResetPassword", "ResetPassword", new { userId = user.Id, code = HttpUtility.UrlEncode(token), language = "en"}, "http");
            
            var body = $@"Reset password: <a href='{passwordResetUrl}'> Reset </a>";
    
            await UserManager.SendEmailAsync(user.Id, "Vuestorefront password reset.", body);
            await _mailService.SendAsync(new IdentityMessage
            {
                Destination = user.Email,
                Body = body,
                Subject = "Vuestorefront password reset"
            });

            return true;
        }
    }
}