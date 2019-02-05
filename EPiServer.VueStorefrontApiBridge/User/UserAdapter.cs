using System.Threading.Tasks;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNet.Identity;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public abstract class UserAdapter<TUser> : IUserAdapter where TUser : ApplicationUser, new()
    {
        protected readonly ApplicationUserManager<TUser> AppUserManager;

        public UserAdapter(ApplicationUserManager<TUser> appUserManager)
        {
            AppUserManager = appUserManager;
        }

        public abstract Task<ApplicationUser> CreateNewUserObject(string userEmail);


        public Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return AppUserManager.CreateAsync((TUser)user, password);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return AppUserManager.UpdateAsync((TUser)user);
        }

        public Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            return AppUserManager.ChangePasswordAsync(userId, oldPassword, newPassword);
        }

        public async Task<ApplicationUser> FindAsync(string userLogin, string userPassword)
        {
            return await AppUserManager.FindAsync(userLogin, userPassword);
        }

        public async Task<ApplicationUser> FindByIdAsync(string id)
        {
            return await AppUserManager.FindByIdAsync(id);
        }
    }
}