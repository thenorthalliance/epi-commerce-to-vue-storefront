using System.Collections.Generic;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{
    public class MerchantReceivableBreakdown
    {
        public ExchangeRate ExchangeRate;

        public Core.Models.PayPal.Orders.Money GrossAmount;

        public Core.Models.PayPal.Orders.Money NetAmount;

        public Core.Models.PayPal.Orders.Money PaypalFee;

        public List<Core.Models.PayPal.Orders.PlatformFee> PlatformFees;

        public Core.Models.PayPal.Orders.Money ReceivableAmount;
    }
}
