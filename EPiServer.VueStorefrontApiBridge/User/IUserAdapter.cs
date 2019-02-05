using System.Threading.Tasks;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNet.Identity;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public interface IUserAdapter
    {
        Task<ApplicationUser> CreateNewUserObject(string userEmail);

        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task<IdentityResult> UpdateAsync(ApplicationUser user);

        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        Task<ApplicationUser> FindAsync(string userLogin, string userPassword);
        Task<ApplicationUser> FindByIdAsync(string id);
    }
}