using System.Collections.Generic;
using System.Web.Http;
using EPiServer.VueStorefrontApiBridge.ApiModel;

namespace EPiServer.VueStorefrontApiBridge.Controllers
{
    public class VsfCartController : ApiController
    {
        public IHttpActionResult Create()
        {
            return Ok(new VsfSuccessResponse<string>("test_cart"));
        }

        [HttpGet]
        [ActionName("payment-methods")]
        public IHttpActionResult PaymentMethods()
        {
            return Ok(new VsfSuccessResponse<List<object>>(new List<object>()));
        }

        [HttpGet]
        public IHttpActionResult Pull()
        {
            return Ok(new VsfSuccessResponse<List<object>>(new List<object>()));
        }
    }
}