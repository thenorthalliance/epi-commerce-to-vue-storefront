using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Orders;

namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class Refund
    {
        public Money Amount;

        public string CreateTime;

        public string Id;

        public string InvoiceId;

        public List<LinkDescription> Links;

        public string NoteToPayer;

        public MerchantPayableBreakdown SellerPayableBreakdown;

        public string Status;

        public StatusDetails StatusDetails;

        public string UpdateTime;
    }
}
