using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.ApiModel;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public interface IUserAdapter
    {
        Task<UserModel> GetUserByCredentials(string userLogin, string userPassword);
        Task<UserModel> GetUserById(string userId);

        Task<UserModel> CreateUser(UserCreateModel newUser);

        Task<bool> ChangePassword(string userId, string oldPassword, string newPassword);
        Task<bool> SendResetPasswordEmail(string userEmail);
        Task<bool> UpdateUser(string userId, UserModel user);
    }
}