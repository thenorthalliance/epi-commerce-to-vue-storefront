namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
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
