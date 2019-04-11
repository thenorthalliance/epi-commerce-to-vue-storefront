using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.VueStorefrontApiBridge.Endpoints
{
    public interface IUserEndpoint
    {
        Task<VsfResponse> CreateLoginResponse(UserLoginModel userLoginModel);
        Task<VsfResponse> RefreshToken(UserRefreshTokenModel userRefreshTokenModel);

        Task<VsfResponse> CreateUser(UserCreateModel userCreateModel);

        Task<VsfResponse> ResetPassword(ResetPasswordModel resetPasswordModel);

        Task<VsfResponse> ChangePassword(string userId, ChangePasswordModel changePasswordModel);
        Task<VsfResponse> GetUser(string userId);
        Task<VsfResponse> UpdateUser(string userId, UserUpdateModel userUpdateModel);
    }
}