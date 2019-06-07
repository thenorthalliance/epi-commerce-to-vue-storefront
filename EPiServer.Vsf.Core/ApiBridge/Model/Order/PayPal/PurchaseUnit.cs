using System.Collections.Generic;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{ 
    public class PurchaseUnit
    {
        public AmountWithBreakdown Amount;

        public string CustomId;

        public string Description;

        public string InvoiceId;

        public List<Item> Items;

        public Payee Payee;

        public PaymentInstruction PaymentInstruction;

        public PaymentCollection Payments;

        public string ReferenceId;

        public ShippingDetails Shipping;

        public string SoftDescriptor;
    }
}
