using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.Vsf.Core.ApiBridge.Adapter
{
    public interface IUserAdapter<TUser> where TUser: UserModel
    {
        Task<TUser> GetUserByCredentials(string userLogin, string userPassword);
        Task<TUser> GetUserById(string userId);
        Task<TUser> CreateUser(UserCreateModel newUser);
        Task<bool> UpdateUser(string userId, UserModel updatedUser);
        Task<bool> ChangePassword(string userId, string oldPassword, string newPassword);
        Task<bool> SendResetPasswordEmail(string userEmail);
    }
}