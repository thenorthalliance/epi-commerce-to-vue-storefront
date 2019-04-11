using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Endpoint;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.Cart;
using Mediachase.Commerce.Customers;

namespace EPiServer.VueStorefrontApiBridge.Endpoints
{
    public class CartEndpoint : ICartEndpoint
    {
        private readonly ICartAdapter _cartAdapter;

        public CartEndpoint(ICartAdapter cartAdapter)
        {
            _cartAdapter = cartAdapter;
        }

        public Task<VsfResponse> CreateCart()
        {
            return Task.FromResult((VsfResponse) new VsfSuccessResponse<string>(_cartAdapter.CreateCart(CustomerContext.Current.CurrentContactId)));
        }

        public Task<VsfResponse> PaymentMethods(Guid cartId)
        {
            var paymentMethods = _cartAdapter.GetPaymentMethods(cartId);
            return Task.FromResult((VsfResponse) new VsfSuccessResponse<IEnumerable<PaymentMethod>>(paymentMethods));
        }

        public Task<VsfResponse> Pull(Guid cartId)
        {
            return Task.FromResult((VsfResponse) new VsfSuccessResponse<IEnumerable<CartItem>>(_cartAdapter.Pull(cartId)));
        }

        public Task<VsfResponse> Update(Guid cartId, CartRequest request)
        {
            if (request.CartItem == null || request.CartItem.Sku.IsNullOrEmpty())
                return Task.FromResult((VsfResponse)new VsfErrorResponse("Cart item is empty"));

            return Task.FromResult((VsfResponse)new VsfSuccessResponse<CartItem>(
                _cartAdapter.Update(cartId, request.CartItem)));
        }

        public Task<VsfResponse> Delete(Guid cartId, CartRequest request)
        {
            var isDeleted = _cartAdapter.Delete(cartId, request.CartItem);
            return Task.FromResult((VsfResponse) new VsfSuccessResponse<bool>(isDeleted));
        }

        public Task<VsfResponse> Totals(Guid cartId)
        {
            var totals = _cartAdapter.GetTotals(cartId);
            return Task.FromResult((VsfResponse)new VsfSuccessResponse<Total>(totals));
        }

        public Task<VsfResponse> ShippingMethods(Guid cartId, ShipmentMethodRequest request)
        {
            var shippingMethods = _cartAdapter.GetShippingMethods(cartId, request.Address);
            return Task.FromResult((VsfResponse) new VsfSuccessResponse<IEnumerable<ShippingMethod>>(shippingMethods));
        }

        public Task<VsfResponse> ShippingInformation(Guid cartId, ShippingInformationRequest request)
        {
            //TODO::: 
            var result = new VsfSuccessResponse<ShippingInformation>(new ShippingInformation
            {
                PaymentMethods = new List<PaymentMethod>(),
                Totals = new Total
                {
                    Items = new List<TotalItem>(),
                    TotalSegments = new List<TotalSegment>()
                }
            });

            return Task.FromResult((VsfResponse) result);
        }

        public Task<VsfResponse> CollectTotals(Guid cartId, CollectTotalsRequest request)
        {
            var totals = _cartAdapter.GetTotals(cartId);
            return Task.FromResult((VsfResponse) new VsfSuccessResponse<Total>(totals));
        }

        public Task<VsfResponse> ApplyCoupon(Guid cartId, string coupon)
        {
            var isAdded = _cartAdapter.AddCoupon(cartId, coupon);
            return Task.FromResult((VsfResponse)new VsfSuccessResponse<bool>(isAdded));
        }

        public Task<VsfResponse> DeleteCoupon(Guid cartId)
        {
            var isDeleted = _cartAdapter.DeleteCoupon(cartId);
            return Task.FromResult((VsfResponse)new VsfSuccessResponse<bool>(isDeleted));
        }

        public Task<VsfResponse> Coupon(Guid cartId)
        {
            var couponCode = _cartAdapter.GetCartCoupon(cartId);
            return Task.FromResult((VsfResponse) new VsfSuccessResponse<string>(couponCode));
        }
    }
}