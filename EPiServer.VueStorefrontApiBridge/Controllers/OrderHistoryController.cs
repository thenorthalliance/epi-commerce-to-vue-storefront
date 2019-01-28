using System.Web.Http;
using EPiServer.VueStorefrontApiBridge.ApiModel;

namespace EPiServer.VueStorefrontApiBridge.Controllers
{
    public class VsfCartController : ApiController
    {
        public IHttpActionResult Create()
        {
            return Ok(new VsfSuccessResponse<string>("fake_cart_id"));
        }
    }
}