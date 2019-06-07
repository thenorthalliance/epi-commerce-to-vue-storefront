using System;

namespace EPiServer.Vsf.Core.Models.PayPal
{
    public class PayPalCreateOrder
    {
        public Guid CartId { get; set; }
        public PayPalShippingData ShippingData { get; set; }
        public PayPalTransaction Transaction { get; set; }
    }
}
