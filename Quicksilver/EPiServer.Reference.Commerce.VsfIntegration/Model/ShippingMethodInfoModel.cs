using System;

namespace EPiServer.Reference.Commerce.VsfIntegration.Model
{
    public class ShippingMethodInfoModel
    {
        public Guid MethodId { get; set; }
        public string ClassName { get; set; }
        public string LanguageId { get; set; }
        public string Currency { get; set; }
        public int Ordering { get; set; }
    }
}