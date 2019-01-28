using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.ApiModel;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public interface IUserAdapter
    {
        Task<UserModel> GetUserByCredentials(string userLogin, string userPassword);
        Task<UserModel> GetUserById(string userId);

        Task<UserModel> CreareUser(UserCreateModel newUser);
    }
}