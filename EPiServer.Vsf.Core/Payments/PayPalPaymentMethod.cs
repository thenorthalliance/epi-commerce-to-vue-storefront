using System;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Dto;

namespace EPiServer.Vsf.Core.Payments
{
    [ServiceConfiguration(typeof(IPaymentMethod))]
    public class PayPalPaymentMethod : IPaymentMethod
    {
        private readonly IOrderGroupFactory _orderGroupFactory;
        private readonly PaymentMethodDto _paymentMethodDto;
        private readonly PaymentMethodDto.PaymentMethodRow _paymentMethod;

        public Guid PaymentMethodId { get; }
        public string SystemKeyword { get; }
        public string Name { get; }
        public string Description { get; }

        public PayPalPaymentMethod() : this(ServiceLocator.Current.GetInstance<IOrderGroupFactory>())
        {
        }

        public PayPalPaymentMethod(IOrderGroupFactory orderGroupFactory)
        {
            _orderGroupFactory = orderGroupFactory;
            _paymentMethodDto = PayPalConfiguration.GetPayPalPaymentMethod();
            _paymentMethod = _paymentMethodDto?.PaymentMethod?.FirstOrDefault();

            if (_paymentMethod == null)
            {
                return;
            }

            PaymentMethodId = _paymentMethod.PaymentMethodId;
            SystemKeyword = _paymentMethod.SystemKeyword;
            Name = _paymentMethod.Name;
            Description = _paymentMethod.Description;
        }

        public IPayment CreatePayment(decimal amount, IOrderGroup orderGroup)
        {
            var payment = orderGroup.CreatePayment(_orderGroupFactory, typeof(PayPalPayment));

            var paymentAction = _paymentMethodDto?.PaymentMethodParameter?.Select(
                $"Parameter = '{PayPalConfiguration.PaymentActionParameter}'").FirstOrDefault() as PaymentMethodDto.PaymentMethodParameterRow;

            payment.PaymentMethodId = _paymentMethod.PaymentMethodId;
            payment.PaymentMethodName = _paymentMethod.Name;
            payment.Amount = amount;
            payment.Status = PaymentStatus.Pending.ToString();

            if (paymentAction != null && paymentAction.Value == TransactionType.Authorization.ToString())
            {
                payment.TransactionType = TransactionType.Authorization.ToString();
            }
            else
            {
                payment.TransactionType = TransactionType.Capture.ToString();
            }

            return payment;
        }

        public bool ValidateData()
        {
            return true;
        }
    }
}