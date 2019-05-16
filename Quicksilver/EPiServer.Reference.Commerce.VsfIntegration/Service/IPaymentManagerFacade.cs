using System;
using Mediachase.Commerce.Orders.Dto;

namespace EPiServer.Reference.Commerce.VsfIntegration.Service
{
    public interface IPaymentManagerFacade
    {
        PaymentMethodDto GetPaymentMethodBySystemName(string name, string languageId);

        PaymentMethodDto GetPaymentMethodsByMarket(string marketId);

        PaymentMethodDto GetPaymentMethodsByMarket(string marketId, string languageId);

        void SavePaymentMethod(PaymentMethodDto dto);
    }
}