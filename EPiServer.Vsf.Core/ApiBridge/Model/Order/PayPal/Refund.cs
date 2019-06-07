using System.Collections.Generic;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{
    public class Refund
    {
        public Core.Models.PayPal.Orders.Money Amount;

        public string CreateTime;

        public string Id;

        public string InvoiceId;

        public List<Core.Models.PayPal.Orders.LinkDescription> Links;

        public string NoteToPayer;

        public MerchantPayableBreakdown SellerPayableBreakdown;

        public string Status;

        public StatusDetails StatusDetails;

        public string UpdateTime;
    }
}
