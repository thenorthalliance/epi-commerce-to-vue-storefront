using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.Vsf.ApiBridge.Authorization;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Authorization;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using Microsoft.AspNet.Identity;

namespace EPiServer.Vsf.ApiBridge.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserEndpoint _userEndpoint;

        public UserController(IUserEndpoint userEndpoint)
        {
            _userEndpoint = userEndpoint;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Login([FromBody]UserLoginModel userLoginModel)
        {
            return Ok(await _userEndpoint.CreateLoginResponse(userLoginModel));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Refresh([FromBody] UserRefreshTokenModel userRefreshTokenModel)
        {
            return Ok(await _userEndpoint.RefreshToken(userRefreshTokenModel));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create(UserCreateModel userCreateModel)
        {
            return Ok(await _userEndpoint.CreateUser(userCreateModel));
        }

        [HttpPost]
        [ActionName("reset-password")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            return Ok(await _userEndpoint.ResetPassword(resetPasswordModel));
        }

        [VsfAuthorize]
        [ActionName("change-password")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            return Ok(await _userEndpoint.ChangePassword(User.Identity.GetUserId(), changePasswordModel));
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
        [ActionName("me")]
        public async Task<IHttpActionResult> GetUser()
        {
            return Ok(await _userEndpoint.GetUser(User.Identity.GetUserId()));
        }

        [HttpPost]
        [VsfAuthorize]
        [ActionName("me")]
        public async Task<IHttpActionResult> UpdateUser(UserUpdateModel updateModel)
        {
            return base.Ok(_userEndpoint.UpdateUser(User.Identity.GetUserId(), updateModel));
        }
    }
}