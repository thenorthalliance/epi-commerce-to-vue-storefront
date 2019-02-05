using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EPiServer.Logging;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public class DefaultUserManager : IUserManager
    {        
        protected readonly IUserMapper UserMapper;
        protected readonly IUserAdapter UserAdapter;
        protected readonly IResetPasswordEmailSender ResetPasswordEmailSender;
        
        public DefaultUserManager(IUserMapper userMapper, IUserAdapter userAdapter, IResetPasswordEmailSender resetPasswordEmailSender)
        {
            UserMapper = userMapper;
            UserAdapter = userAdapter;
            ResetPasswordEmailSender = resetPasswordEmailSender;
        }

        public async Task<UserModel> GetUserByCredentials(string userLogin, string userPassword)
        {
            return UserMapper.MapUser(await UserAdapter.FindAsync(userLogin, userPassword));
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            return UserMapper.MapUser(await UserAdapter.FindByIdAsync(userId));
        }

        public async Task<UserModel> CreateUser(UserCreateModel newUser)
        {
            var appUser = await UserAdapter.CreateNewUserObject(newUser.Customer.Email);
            var result = await UserAdapter.CreateAsync(appUser, newUser.Password);

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

            var user = await UserAdapter.FindByIdAsync(appUser.Id);
            return UserMapper.MapUser(user);
        }

        public async Task<bool> UpdateUser(string userId, UserModel updatedUser)
        {
            var user = await UserAdapter.FindByIdAsync(userId);
            user.UserName = updatedUser.Email;
            user.Email = updatedUser.Email;

            var result = await UserAdapter.UpdateAsync(user);
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
            var result = await UserAdapter.ChangePasswordAsync(userId, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                LogDebugErrors($"Change password for user '{userId}' failed", result.Errors);
                return false;
            }

            return true;
        }

        public Task<bool> SendResetPasswordEmail(string userEmail)
        {
            return ResetPasswordEmailSender.Send(userEmail);
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