using System;
using System.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.Authorization;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;
using EPiServer.VueStorefrontApiBridge.User;
using Microsoft.AspNet.Identity;

namespace EPiServer.VueStorefrontApiBridge.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserProvider _provider;

        public UserController(IUserProvider provider)
        {
            _provider = provider;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Login([FromBody]UserLoginModel userLoginModel)
        {
            var user = await _provider.GetUserByCredentials(userLoginModel.username, userLoginModel.password);

            if(user == null)
                return Ok(new VsfErrorResponse("You did not sign in correctly or your account is temporarily disabled."));

            var tokenProvider = GetTokenProvider();
            var authToken = await tokenProvider.GenerateNewToken(user);
            var refreshToken = await tokenProvider.GenerateNewRefreshToken(user);

            return Ok(new LoginResponse(authToken, refreshToken));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Refresh([FromBody] UserRefreshTokenModel userRefreshTokenModel)
        {
            var tokenProvider = GetTokenProvider();
            var refreshToken = await tokenProvider.GetRefreshToken(userRefreshTokenModel.refreshToken);

            var user = await _provider.GetUserById(refreshToken.UserId);
            var authToken = await tokenProvider.GenerateNewToken(user);

            return Ok(new RefreshTokenResponse(authToken));
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
            return Ok(new VsfSuccessResponse<UserModel>(await _provider.GetUserById(
                User.Identity.GetUserId())));
        }

        private static JwtUserTokenProvider GetTokenProvider()
        {
            return new JwtUserTokenProvider(new AuthTokenOptions
            {
                Issuer = "test_issuer",
                Audience = "http://localhost:50244",
                SecurityKey = new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes("alamakotaalamakotaalamakotaalamakota"))
            }, new MemoryRefreshTokenRepo());
        }
    }
}