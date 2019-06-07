namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{
    public class AmountWithBreakdown
    {
        public AmountBreakdown Breakdown;

        public string CurrencyCode;

        public string Value;
    }
}
