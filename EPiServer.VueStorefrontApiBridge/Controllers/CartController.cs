using System;
using System.Collections.Generic;
using System.Web.Http;
using Castle.Core.Internal;
using EPiServer.VueStorefrontApiBridge.Adapter.Cart;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.ApiModel.Cart;
using Mediachase.Commerce.Customers;

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
            return Ok(new VsfSuccessResponse<string>(_iCartAdapter.CreateCart(CustomerContext.Current.CurrentContactId)));
        }

        [HttpGet]
        [ActionName("payment-methods")]
        public IHttpActionResult PaymentMethods(Guid cartId)
        {
            var paymentMethods = _iCartAdapter.GetPaymentMethods(cartId);
            return Ok(new VsfSuccessResponse<IEnumerable<PaymentMethod>>(paymentMethods));
        }

        [HttpGet]
        public IHttpActionResult Pull(Guid cartId)
        {
            return Ok(new VsfSuccessResponse<IEnumerable<CartItem>>(_iCartAdapter.Pull(cartId)));
        }

        [HttpPost]
        public IHttpActionResult Update(Guid cartId, [FromBody] CartRequest request)
        {
            if (request.CartItem == null || request.CartItem.Sku.IsNullOrEmpty())
            {
                return Ok(new VsfErrorResponse("Cart item is empty"));
            }
            return Ok(new VsfSuccessResponse<CartItem>(
                _iCartAdapter.Update(cartId, request.CartItem)));
        }

        [HttpPost]
        public IHttpActionResult Delete(Guid cartId, [FromBody] CartRequest request)
        {
            var isDeleted = _iCartAdapter.Delete(cartId, request.CartItem);
            return Ok(new VsfSuccessResponse<bool>(isDeleted));
        }

        [HttpGet]
        public IHttpActionResult Totals(Guid cartId)
        {
            var totals = _iCartAdapter.GetTotals(cartId);
            return Ok(new VsfSuccessResponse<Total>(totals));
        }

        [HttpPost]
        [ActionName("shipping-methods")]
        public IHttpActionResult ShippingMethods(Guid cartId, [FromBody] ShipmentMethodRequest request)
        {
            var shippingMethods = _iCartAdapter.GetShippingMethods(cartId, request.Address);
            return Ok(new VsfSuccessResponse<IEnumerable<ShippingMethod>>(shippingMethods));

        }

        [HttpPost]
        [ActionName("shipping-information")]
        public IHttpActionResult ShippingInformation(Guid cartId, [FromBody] ShippingInformationRequest request)
        {
            //TODO::: 
            return Ok(new VsfSuccessResponse<ShippingInformation>(null));
        }

        [HttpPost]
        [ActionName("collect-totals")]
        public IHttpActionResult CollectTotals(Guid cartId, CollectTotalsRequest request)
        {
            //TODO::: Not sure what we should do here
            var totals = _iCartAdapter.GetTotals(cartId);
            return Ok(new VsfSuccessResponse<Total>(totals));
        }

        [HttpPost]
        [ActionName("apply-coupon")]
        public IHttpActionResult ApplyCoupon(Guid cartId, string coupon)
        {
            var isAdded = _iCartAdapter.AddCoupon(cartId, coupon);
            return Ok(new VsfSuccessResponse<bool>(isAdded));
        }

        [HttpPost]
        [ActionName("delete-coupon")]
        public IHttpActionResult DeleteCoupon(Guid cartId)
        {
            var isDeleted = _iCartAdapter.DeleteCoupon(cartId);
            return Ok(new VsfSuccessResponse<bool>(isDeleted));
        }

        [HttpGet]
        [ActionName("coupon")]
        public IHttpActionResult Coupon(Guid cartId)
        {
            var couponCode = _iCartAdapter.GetCartCoupon(cartId);
            return Ok(new VsfSuccessResponse<string>(couponCode));
        }

    }
}