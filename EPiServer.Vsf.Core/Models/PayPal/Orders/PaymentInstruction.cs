using System.Collections.Generic;

namespace EPiServer.Vsf.Core.Models.PayPal.Orders
{
    public class PaymentInstruction
    {
        public string DisbursementMode;

        public List<PlatformFee> PlatformFees;
    }
}
