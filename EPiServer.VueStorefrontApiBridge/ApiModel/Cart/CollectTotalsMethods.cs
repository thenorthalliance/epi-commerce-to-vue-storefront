namespace EPiServer.VueStorefrontApiBridge.ApiModel.Cart
{
    public class CollectTotalsMethods
    {
        public PaymentMethodRequestModel PaymentMethod { get; set; }

        public string ShippingMethodCode { get; set; }

        public string ShippingCarrierCode { get; set; }
    }
}
