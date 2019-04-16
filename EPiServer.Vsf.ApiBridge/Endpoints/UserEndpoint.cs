using System.Linq;
using System.Threading.Tasks;
using EPiServer.Vsf.ApiBridge.Authorization.Claims;
using EPiServer.Vsf.ApiBridge.Authorization.Token;
using EPiServer.Vsf.ApiBridge.Utils;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Authorization;
using EPiServer.Vsf.Core.ApiBridge.Model.User;

namespace EPiServer.Vsf.ApiBridge.Endpoints
{
    public class UserEndpoint<TUser>  : IUserEndpoint where TUser : VsfUser
    {
        private readonly IUserAdapter<TUser> _userAdapter;
        private readonly IUserTokenProvider _userTokenProvider;
        private readonly IUserClaimsProvider<TUser> _userClaimsProvider;

        public UserEndpoint(IUserAdapter<TUser> userAdapter, IUserTokenProvider userTokenProvider, IUserClaimsProvider<TUser> userClaimsProvider)
        {
            _userAdapter = userAdapter;
            _userTokenProvider = userTokenProvider;
            _userClaimsProvider = userClaimsProvider;
        }

        public async Task<VsfResponse> CreateLoginResponse(UserLoginModel userLoginModel)
        {
            var user = await _userAdapter.GetUserByCredentials(userLoginModel.Username, userLoginModel.Password);
            if (user == null)
                return new VsfErrorResponse("You did not sign in correctly or your account is temporarily disabled.");

            using (await UserLocker.LockAsync(user.Id))
            {
                var userClaims = _userClaimsProvider.GetClaims(user).ToList();
                var authToken = await _userTokenProvider.GenerateNewToken(userClaims);
                var refreshToken = await _userTokenProvider.GenerateNewRefreshToken(userClaims);

                return new LoginResponse(authToken, refreshToken);
            }
        }

        public async Task<VsfResponse> RefreshToken(UserRefreshTokenModel userRefreshTokenModel)
        {
            var refreshToken = await _userTokenProvider.GetRefreshToken(userRefreshTokenModel.RefreshToken);

            var user = await _userAdapter.GetUserById(refreshToken.UserId);
            var userClaims = _userClaimsProvider.GetClaims(user).ToList();
            var authToken = await _userTokenProvider.GenerateNewToken(userClaims);

            return new RefreshTokenResponse(authToken);
        }

        public async Task<VsfResponse> CreateUser(UserCreateModel userCreateModel)
        {
            var newUser = await _userAdapter.CreateUser(userCreateModel);
            if (newUser == null)
                return new VsfErrorResponse("User not created. TODO ADD INFO!");

            return new VsfSuccessResponse<VsfUser>(newUser);
        }

        public async Task<VsfResponse> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (!await _userAdapter.SendResetPasswordEmail(resetPasswordModel.Email))
                return new VsfErrorResponse($"No such entity with email = {resetPasswordModel.Email}");
            return new VsfSuccessResponse<string>("Email sent.");
        }

        public async Task<VsfResponse> ChangePassword(string userId, ChangePasswordModel changePasswordModel)
        {
            using (await UserLocker.LockAsync(userId))
            {
                if (!await _userAdapter.ChangePassword(userId,
                    changePasswordModel.CurrentPassword, changePasswordModel.NewPassword))
                    return new VsfErrorResponse("The password doesn't match this account.");

                return new VsfSuccessResponse<string>("Password changed.");
            }
        }

        public async Task<VsfResponse> GetUser(string userId)
        {
            using (await UserLocker.LockAsync(userId))
            {
                return new VsfSuccessResponse<VsfUser>(await _userAdapter.GetUserById(userId));
            }
        }

        public async Task<VsfResponse> UpdateUser(string userId, UserUpdateModel userUpdateModel)
        {
            using (await UserLocker.LockAsync(userId))
            {
                if (await _userAdapter.UpdateUser(userId, userUpdateModel.Customer))
                    return new VsfSuccessResponse<VsfUser>(await _userAdapter.GetUserById( userId));

                return new VsfErrorResponse("User update failed.");
            }
        }
    }
}