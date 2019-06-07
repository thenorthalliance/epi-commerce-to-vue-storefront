using System;

namespace EPiServer.Vsf.Core.Models.PayPal
{
    public class PayPalCaptureRequest
    {
        public string Intent { get; set; }

        public string PaymentId { get; set; }

        public string PayerId { get; set; }

        public string OrderId { get; set; }

        public Guid CartId { get; set; }
    }
}
