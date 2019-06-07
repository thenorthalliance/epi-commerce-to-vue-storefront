using System.Collections.Generic;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{
    public class Capture
    {
        public Core.Models.PayPal.Orders.Money Amount;

        public string CreateTime;

        public string DisbursementMode;

        public bool? FinalCapture;

        public string Id;

        public string InvoiceId;

        public List<Core.Models.PayPal.Orders.LinkDescription> Links;

        public Core.Models.PayPal.Orders.SellerProtection SellerProtection;

        public MerchantReceivableBreakdown SellerReceivableBreakdown;

        public string Status;

        public StatusDetails StatusDetails;

        public string UpdateTime;
    }
}
