namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class AmountBreakdown
    {
        public Money Handling;

        public Money Insurance;

        public Money ItemTotal;

        public Money Shipping;

        public Money ShippingDiscount;

        public Money TaxTotal;
    }
}
