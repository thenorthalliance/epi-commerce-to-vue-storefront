using System.Threading.Tasks;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Authorization;
using EPiServer.Vsf.Core.ApiBridge.Model.User;

namespace EPiServer.Vsf.Core.ApiBridge.Endpoint
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