using System;
using System.Threading.Tasks;
using System.Web.Http;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model.Cart;

namespace EPiServer.VueStorefrontApiBridge.Controllers
{
    public class CartController : ApiController
    {
        private readonly ICartEndpoint _cartEndpoint;

        public CartController(ICartEndpoint cartEndpoint)
        {
            _cartEndpoint = cartEndpoint;
        }

        [HttpPost]
        public async Task<IHttpActionResult> Create()
        {
            return Ok(await _cartEndpoint.CreateCart());
        }

        [HttpGet]
        [ActionName("payment-methods")]
        public async Task<IHttpActionResult> PaymentMethods(Guid cartId)
        {
            return Ok(await _cartEndpoint.PaymentMethods(cartId));
        }

        [HttpGet]
        public async Task<IHttpActionResult> Pull(Guid cartId)
        {
            return Ok(await _cartEndpoint.Pull(cartId));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Update(Guid cartId, [FromBody] CartRequest request)
        {
            return Ok(await _cartEndpoint.Update(cartId, request));
        }

        [HttpPost]
        public async Task<IHttpActionResult> Delete(Guid cartId, [FromBody] CartRequest request)
        {
            return Ok(await _cartEndpoint.Delete(cartId, request));
        }

        [HttpGet]
        public async Task<IHttpActionResult> Totals(Guid cartId)
        {
            return Ok(await _cartEndpoint.Totals(cartId));
        }

        [HttpPost]
        [ActionName("shipping-methods")]
        public async Task<IHttpActionResult> ShippingMethods(Guid cartId, [FromBody] ShipmentMethodRequest request)
        {
            return Ok(await _cartEndpoint.ShippingMethods(cartId, request));
        }

        [HttpPost]
        [ActionName("shipping-information")]
        public async Task<IHttpActionResult> ShippingInformation(Guid cartId, [FromBody] ShippingInformationRequest request)
        {
            return Ok(await _cartEndpoint.ShippingInformation(cartId, request));
        }

        [HttpPost]
        [ActionName("collect-totals")]
        public async Task<IHttpActionResult> CollectTotals(Guid cartId, CollectTotalsRequest request)
        {
            return Ok(await _cartEndpoint.CollectTotals(cartId, request));
        }

        [HttpPost]
        [ActionName("apply-coupon")]
        public async Task<IHttpActionResult> ApplyCoupon(Guid cartId, string coupon)
        {
            return Ok(await _cartEndpoint.ApplyCoupon(cartId, coupon));
        }

        [HttpPost]
        [ActionName("delete-coupon")]
        public async Task<IHttpActionResult> DeleteCoupon(Guid cartId)
        {
            return Ok(await _cartEndpoint.DeleteCoupon(cartId));
        }

        [HttpGet]
        [ActionName("coupon")]
        public async Task<IHttpActionResult> Coupon(Guid cartId)
        {
            return Ok(await _cartEndpoint.Coupon(cartId));
        }
    }
}