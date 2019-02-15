using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.Reference.Commerce.Shared.Services;
using EPiServer.ServiceLocation;
using EPiServer.VueStorefrontApiBridge.Adapter.User;
using EPiServer.VueStorefrontApiBridge.Utils;
using Microsoft.AspNet.Identity;

namespace EPiServer.Reference.Commerce.Site.Infrastructure
{
  
    [ServiceConfiguration(typeof(IUserAdapter), Lifecycle = ServiceInstanceScope.Transient)]
    [ServiceConfiguration(typeof(IResetPasswordEmailSender), Lifecycle = ServiceInstanceScope.Transient)]
    public class QuickSilverUserAdapter : UserAdapter<SiteUser>, IResetPasswordEmailSender
    {
        private readonly MailService _mailService;
        private readonly UrlHelper _urlHelper;

        public QuickSilverUserAdapter(MailService mailService, UrlHelper urlHelper, ApplicationUserManager<SiteUser> appUserManager) : base(appUserManager)
        {
            _mailService = mailService;
            _urlHelper = urlHelper;
        }

        public override Task<ApplicationUser> CreateNewUserObject(string userEmail)
        {
            return Task.FromResult((ApplicationUser)new SiteUser
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

        async Task<bool> IResetPasswordEmailSender.Send(string userEmail)
        {
            //TEST RESETPASWORD EMAIL SENDING
            var user = await AppUserManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                LogDebugErrors($"SendResetPasswordEmail: User '{userEmail}' not found");
                return false;
            }

            var token = await AppUserManager.GeneratePasswordResetTokenAsync(user.Id);
            var passwordResetUrl = _urlHelper.Action("ResetPassword", "ResetPassword", new { userId = user.Id, code = HttpUtility.UrlEncode(token), language = "en"}, "http");
            
            var body = $@"Reset password: <a href='{passwordResetUrl}'> Reset </a>";
    
            await AppUserManager.SendEmailAsync(user.Id, "Vuestorefront password reset.", body);
            await _mailService.SendAsync(new IdentityMessage
            {
                Destination = user.Email,
                Body = body,
                Subject = "Vuestorefront password reset"
            });

            return true;
        }

        protected void LogDebugErrors(string message, IEnumerable<string> errors = null)
        {
            var errorMessage = errors != null ?
                $"{message}:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}" :
                message;

            LogManager.GetLogger(GetType()).Debug(errorMessage);
        }
    }
}