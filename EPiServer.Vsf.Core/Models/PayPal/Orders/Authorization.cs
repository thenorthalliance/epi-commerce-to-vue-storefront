using System.Collections.Generic;

namespace EPiServer.Vsf.Core.Models.PayPal.Orders
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
