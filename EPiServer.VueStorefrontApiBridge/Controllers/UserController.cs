using System;
using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.Authorization;
using EPiServer.VueStorefrontApiBridge.User;
using Microsoft.AspNet.Identity;

namespace EPiServer.VueStorefrontApiBridge.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserAdapter _userAdapter;
        private readonly IUserTokenProvider _tokenProvider;

        public UserController(IUserAdapter userAdapter, IUserTokenProvider userTokenProvider)
        {
            _userAdapter = userAdapter;
            _tokenProvider = userTokenProvider;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Login([FromBody]UserLoginModel userLoginModel)
        {
            var user = await _userAdapter.GetUserByCredentials(userLoginModel.Username, userLoginModel.Password);

            if(user == null)
                return Ok(new VsfErrorResponse("You did not sign in correctly or your account is temporarily disabled."));

            var authToken = await _tokenProvider.GenerateNewToken(user);
            var refreshToken = await _tokenProvider.GenerateNewRefreshToken(user);

            return Ok(new LoginResponse(authToken, refreshToken));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Refresh([FromBody] UserRefreshTokenModel userRefreshTokenModel)
        {
            var refreshToken = await _tokenProvider.GetRefreshToken(userRefreshTokenModel.RefreshToken);

            var user = await _userAdapter.GetUserById(refreshToken.UserId);
            var authToken = await _tokenProvider.GenerateNewToken(user);

            return Ok(new RefreshTokenResponse(authToken));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(UserCreateModel userCreateModel)
        {
            var newUser = await _userAdapter.CreateUser(userCreateModel);
            return Ok(new VsfSuccessResponse<UserModel>(newUser));
        }

        [HttpPost]
        public IHttpActionResult ResetPassword()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IHttpActionResult ChangePassword()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [ActionName("order-history")]
        [VsfAuthorize]
        public IHttpActionResult OrderHistory()
        {
            return Ok(new VsfSuccessResponse<OrderHistoryModel>(new OrderHistoryModel()));
        }

        [HttpGet]
        [VsfAuthorize]
        public async Task<IHttpActionResult> Me()
        {
            return Ok(new VsfSuccessResponse<UserModel>(await _userAdapter.GetUserById(
                User.Identity.GetUserId())));
        }
    }
}