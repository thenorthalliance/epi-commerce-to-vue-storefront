namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{
    public class NetAmountBreakdownItem
    {
        public Core.Models.PayPal.Orders.Money ConvertedAmount;

        public ExchangeRate ExchangeRate;

        public Core.Models.PayPal.Orders.Money PayableAmount;
    }
}
