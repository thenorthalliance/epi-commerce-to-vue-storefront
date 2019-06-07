using System.Collections.Generic;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{

    public class PaymentCollection
    {
        public List<Authorization> Authorizations;

        public List<Capture> Captures;

        public List<Refund> Refunds;
    }
}
