using System.Collections.Generic;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{
    public class PaymentInstruction
    {
        public string DisbursementMode;

        public List<PlatformFee> PlatformFees;
    }
}
