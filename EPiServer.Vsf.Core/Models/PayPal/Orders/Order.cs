using System.Collections.Generic;

namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class Order
    {
        public string CreateTime;

        public string Id;

        public string Intent;

        public List<LinkDescription> Links;

        public Customer Payer;

        public List<PurchaseUnit> PurchaseUnits;

        public string Status;

        public string UpdateTime;
    }
}
