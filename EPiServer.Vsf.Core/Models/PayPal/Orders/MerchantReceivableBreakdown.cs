using System.Collections.Generic;

namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class MerchantReceivableBreakdown
    {
        public ExchangeRate ExchangeRate;

        public Money GrossAmount;

        public Money NetAmount;

        public Money PaypalFee;

        public List<PlatformFee> PlatformFees;

        public Money ReceivableAmount;
    }
}
