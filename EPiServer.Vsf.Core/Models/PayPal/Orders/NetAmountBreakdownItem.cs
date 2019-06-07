namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class NetAmountBreakdownItem
    {
        public Money ConvertedAmount;

        public ExchangeRate ExchangeRate;

        public Money PayableAmount;
    }
}
