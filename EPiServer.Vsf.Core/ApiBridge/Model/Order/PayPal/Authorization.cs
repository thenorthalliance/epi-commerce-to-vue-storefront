using System.Collections.Generic;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{

    public class Authorization
    {
        public Money Amount;

        public string CreateTime;

        public string ExpirationTime;

        public string Id;

        public string InvoiceId;

        public List<LinkDescription> Links;

        public SellerProtection SellerProtection;

        public string Status;

        public string UpdateTime;
    }
}
