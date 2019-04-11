namespace EPiServer.Vsf.Core.ApiBridge.Model.Cart
{
    public class CollectTotalsMethods
    {
        public PaymentMethodRequestModel PaymentMethod { get; set; }

        public string ShippingMethodCode { get; set; }

        public string ShippingCarrierCode { get; set; }
    }
}
