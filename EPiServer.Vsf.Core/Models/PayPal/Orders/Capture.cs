using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Orders;

namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class Capture
    {
        public Money Amount;

        public string CreateTime;

        public string DisbursementMode;

        public bool? FinalCapture;

        public string Id;

        public string InvoiceId;

        public List<LinkDescription> Links;

        public SellerProtection SellerProtection;

        public MerchantReceivableBreakdown SellerReceivableBreakdown;

        public string Status;

        public StatusDetails StatusDetails;

        public string UpdateTime;
    }
}
