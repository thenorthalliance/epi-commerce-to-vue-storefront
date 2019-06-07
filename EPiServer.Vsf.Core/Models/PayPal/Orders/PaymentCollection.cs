using System.Collections.Generic;
using PayPalCheckoutSdk.Orders;

namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class PaymentCollection
    {
        public List<Authorization> Authorizations;

        public List<Capture> Captures;

        public List<Refund> Refunds;
    }
}
