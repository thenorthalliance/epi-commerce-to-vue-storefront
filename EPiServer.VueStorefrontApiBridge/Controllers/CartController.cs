using System.Collections.Generic;
using System.Web.Http;
using Castle.Core.Internal;
using EPiServer.VueStorefrontApiBridge.Adapter.Cart;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.ApiModel.Cart;
using Microsoft.AspNet.Identity;

namespace EPiServer.VueStorefrontApiBridge.Controllers
{
    public class CartController : ApiController
    {
        private readonly ICartAdapter _iCartAdapter;

        public CartController(ICartAdapter iCartAdapter)
        {
            _iCartAdapter = iCartAdapter;
        }

        [HttpPost]
        public IHttpActionResult Create()
        {
            return Ok(new VsfSuccessResponse<string>(_iCartAdapter.CreateCart(GetUserId())));
        }

        [HttpGet]
        [ActionName("payment-methods")]
        public IHttpActionResult PaymentMethods(string cartId)
        {
            var paymentMethods = _iCartAdapter.GetPaymentMethods(GetUserId(), cartId);
            return Ok(new VsfSuccessResponse<IEnumerable<PaymentMethod>>(paymentMethods));
        }

        [HttpGet]
        public IHttpActionResult Pull(string cartId)
        {
            return Ok(new VsfSuccessResponse<IEnumerable<CartItem>>(_iCartAdapter.Pull(GetUserId(), cartId)));
        }

        [HttpPost]
        public IHttpActionResult Update([FromUri ]string token, [FromUri] string cartId, [FromBody] CartRequest request)
        {
            if (request.CartItem == null || request.CartItem.Sku.IsNullOrEmpty())
            {
                return Ok(new VsfErrorResponse("Cart item is empty"));
            }
            return Ok(new VsfSuccessResponse<CartItem>(
                _iCartAdapter.Update(GetUserId(), cartId, request.CartItem)));
        }

        [HttpPost]
        public IHttpActionResult Delete(string cartId, [FromBody] CartRequest request)
        {
            var isDeleted = _iCartAdapter.Delete(GetUserId(), cartId, request.CartItem);
            return Ok(new VsfSuccessResponse<bool>(isDeleted));
        }

        [HttpGet]
        public IHttpActionResult Totals(string cartId)
        {
            var totals = _iCartAdapter.GetTotals(GetUserId(), cartId);
            return Ok(new VsfSuccessResponse<Total>(totals));
        }

        [HttpPost]
        [ActionName("shipping-methods")]
        public IHttpActionResult ShippingMethods(string cartId, [FromBody] ShipmentMethodRequest request)
        {
            var shippingMethods = _iCartAdapter.GetShippingMethods(GetUserId(), cartId, request.Address);
            return Ok(new VsfSuccessResponse<IEnumerable<ShippingMethod>>(shippingMethods));

        }

        [HttpPost]
        [ActionName("shipping-information")]
        public IHttpActionResult ShippingInformation(string cartId, [FromBody] ShippingInformationRequest request)
        {
            //TODO::: 
            return Ok(new VsfSuccessResponse<ShippingInformation>(null));
        }

        [HttpPost]
        [ActionName("collect-totals")]
        public IHttpActionResult CollectTotals(string cartId, CollectTotalsRequest request)
        {
            //TODO::: 
            return Ok(new VsfSuccessResponse<Total>(null));
        }

        [HttpPost]
        [ActionName("apply-coupon")]
        public IHttpActionResult ApplyCoupon(string cartId, string coupon)
        {
            var isAdded = _iCartAdapter.AddCoupon(GetUserId(), cartId, coupon);
            return Ok(new VsfSuccessResponse<bool>(isAdded));
        }

        [HttpPost]
        [ActionName("delete-coupon")]
        public IHttpActionResult DeleteCoupon(string cartId)
        {
            var isDeleted = _iCartAdapter.DeleteCoupon(GetUserId(), cartId);
            return Ok(new VsfSuccessResponse<bool>(isDeleted));
        }

        [HttpGet]
        [ActionName("coupon")]
        public IHttpActionResult Coupon(string cartId)
        {
            var couponCode = _iCartAdapter.GetCartCoupon(GetUserId(), cartId);
            return Ok(new VsfSuccessResponse<string>(couponCode));
        }

        private string GetUserId()
        {
            return User.Identity.IsAuthenticated ? User.Identity.GetUserId() : null;
        }
    }
}