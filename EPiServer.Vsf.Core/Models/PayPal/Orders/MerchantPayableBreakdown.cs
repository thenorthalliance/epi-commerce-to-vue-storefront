using System.Collections.Generic;

namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class MerchantPayableBreakdown
    {
        public Money GrossAmount;

        public Money NetAmount;

        public List<NetAmountBreakdownItem> NetAmountBreakdown;

        public Money PaypalFee;

        public List<PlatformFee> PlatformFees;

        public Money TotalRefundedAmount;
    }
}
