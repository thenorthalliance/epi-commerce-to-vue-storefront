using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Logging;
using EPiServer.Reference.Commerce.Shared.Identity;
using EPiServer.Reference.Commerce.Shared.Services;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;
using Microsoft.AspNet.Identity;

namespace EPiServer.Reference.Commerce.VsfIntegration.Adapter
{
    public class QuickSilverUserAdapter : IUserAdapter<VsfUser>
    {
        private readonly ApplicationUserManager<SiteUser> _appUserManager;
        private readonly MailService _mailService;
        private readonly UrlHelper _urlHelper;

        public QuickSilverUserAdapter(ApplicationUserManager<SiteUser> appUserManager, MailService mailService, UrlHelper urlHelper)
        {
            _appUserManager = appUserManager;
            _mailService = mailService;
            _urlHelper = urlHelper;
        }

        public async Task<VsfUser> GetUserByCredentials(string userLogin, string userPassword)
        {
            var user = await _appUserManager.FindAsync(userLogin, userPassword);
            if (user == null || !user.IsApproved)
                return null;

            return QuicksilverUserModelMapper.MapToVsfModel(user);
        }

        public async Task<VsfUser> GetUserById(string userId)
        {
            var user = await _appUserManager.FindByIdAsync(userId);
            return QuicksilverUserModelMapper.MapToVsfModel(user);
        }

        public async Task<VsfUser> CreateUser(UserCreateModel newUser)
        {
            var appUser = new SiteUser
            {
                Id = Guid.NewGuid().ToString(),
                Username = newUser.Customer.Email,
                Email = newUser.Customer.Email,
                EmailConfirmed = false,
                IsLockedOut = false,
                IsApproved = true,
                CreationDate = DateTime.UtcNow
            };

            var result = await _appUserManager.CreateAsync(appUser, newUser.Password);
            
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

            var user = await _appUserManager.FindByIdAsync(appUser.Id);
            return QuicksilverUserModelMapper.MapToVsfModel(user);
        }

        public async Task<bool> UpdateUser(string userId, VsfUser updatedUser)
        {
            var user = await _appUserManager.FindByIdAsync(userId);
            user.UserName = updatedUser.Email;
            user.Email = updatedUser.Email;

            var identity = await _appUserManager.UpdateAsync(user);

            if (!identity.Succeeded)
            {
                LogDebugErrors("UpdateUser failed", identity.Errors);
                return false;
            }

            return QuicksilverUserModelMapper.UpdateCustomerContact(this, userId, updatedUser);
        }

        public async Task<bool> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var result = await _appUserManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                LogDebugErrors($"Change password for user '{userId}' failed", result.Errors);
                return false;
            }

            return true;
        }

        public async Task<bool> SendResetPasswordEmail(string userEmail)
        {
            //TEST RESETPASWORD EMAIL SENDING
            var user = await _appUserManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                LogDebugErrors($"SendResetPasswordEmail: User '{userEmail}' not found");
                return false;
            }

            var token = await _appUserManager.GeneratePasswordResetTokenAsync(user.Id);
            var passwordResetUrl = _urlHelper.Action("ResetPassword", "ResetPassword", new { userId = user.Id, code = HttpUtility.UrlEncode(token), language = "en" }, "http");

            var body = $@"Reset password: <a href='{passwordResetUrl}'> Reset </a>";

            await _appUserManager.SendEmailAsync(user.Id, "Vuestorefront password reset.", body);
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