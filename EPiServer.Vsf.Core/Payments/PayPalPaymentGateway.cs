using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EPiServer.Commerce.Order;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.Core.Services;
using EPiServer.Web;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Plugins.Payment;
using Mediachase.Commerce.Security;

namespace EPiServer.Vsf.Core.Payments
{
    public class PayPalPaymentGateway : AbstractPaymentGateway, IPaymentPlugin
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(PayPalPaymentGateway));

        public const string PayPalOrderNumberPropertyName = "PayPalOrderNumber";
        public const string PayPalExpTokenPropertyName = "PayPalExpToken";

        private readonly IOrderRepository _orderRepository;
        private readonly IPayPalService _payPalService;

        public PayPalPaymentGateway()
            : this(ServiceLocator.Current.GetInstance<IOrderRepository>(),
                  ServiceLocator.Current.GetInstance<IPayPalService>())
        { }

        public PayPalPaymentGateway(
            IOrderRepository orderRepository,
            IPayPalService payPalService)
        {
            _orderRepository = orderRepository;
            _payPalService = payPalService;
        }

        public override bool ProcessPayment(Payment payment, ref string message)
        {
            var orderGroup = payment.Parent.Parent;

            var paymentProcessingResult = ProcessPayment(orderGroup, payment);

            message = paymentProcessingResult.Message;
            return paymentProcessingResult.IsSuccessful;
        }

        public PaymentProcessingResult ProcessPayment(IOrderGroup orderGroup, IPayment payment)
        {
            if (payment == null)
            {
                return PaymentProcessingResult.CreateUnsuccessfulResult("PaymentNotSpecified");
            }

            var orderForm = orderGroup.Forms.FirstOrDefault(f => f.Payments.Contains(payment));
            if (orderForm == null)
            {
                return PaymentProcessingResult.CreateUnsuccessfulResult("PaymentNotAssociatedOrderForm");
            }

            PaymentProcessingResult paymentProcessingResult;

            var cart = orderGroup as ICart;
            if (cart == null && orderGroup is IPurchaseOrder)
            {
                if (payment.TransactionType == TransactionType.Capture.ToString())
                {
                    var authorizationId = payment.Properties[PayPalConfiguration.PayPalExpToken].ToString();
                    var result = Task.Run(() => _payPalService.CaptureAuthorizationAsync(authorizationId)).GetAwaiter().GetResult();

                    PaymentStatusManager.ProcessPayment(payment);

                    AddNoteToPurchaseOrder("PayPal OrderId", "PayPal OrderId: " + payment.Properties[PayPalConfiguration.PayPalOrderNumber], orderGroup.CustomerId, (IPurchaseOrder)orderGroup);
                    AddNoteToPurchaseOrder("PayPal Authorization Id", "PayPal Authorization Id: " + payment.Properties[PayPalConfiguration.PayPalExpToken], orderGroup.CustomerId, (IPurchaseOrder)orderGroup);
                    AddNoteToPurchaseOrder("PayPal Capture Id", "PayPal Capture Id: " + result.Id, orderGroup.CustomerId, (IPurchaseOrder)orderGroup);

                    _orderRepository.Save(orderGroup);

                    return PaymentProcessingResult.CreateSuccessfulResult("CaptureProcessed");
                }

                paymentProcessingResult = PaymentProcessingResult.CreateUnsuccessfulResult("The current payment method does not support order type.");
                return paymentProcessingResult; // raise exception
            }

            if (cart != null && cart.OrderStatus == OrderStatus.Completed)
            {
                paymentProcessingResult = PaymentProcessingResult.CreateSuccessfulResult("ProcessPaymentStatusCompleted");

                return paymentProcessingResult;
            }

            return PaymentProcessingResult.CreateSuccessfulResult("ProcessPaymentStatusCompleted");
        }

        public string ProcessUnsuccessfulTransaction(string cancelUrl, string errorMessage)
        {
            if (HttpContext.Current == null)
            {
                return cancelUrl;
            }

            _logger.Error($"PayPal transaction failed [{errorMessage}].");
            return UriUtil.AddQueryString(cancelUrl, "message", errorMessage);
        }

        private void AddNoteToPurchaseOrder(string title, string detail, Guid customerId, IPurchaseOrder purchaseOrder)
        {
            var orderNote = purchaseOrder.CreateOrderNote();
            orderNote.Type = OrderNoteTypes.System.ToString();
            orderNote.CustomerId =
                customerId != Guid.Empty ? customerId : PrincipalInfo.CurrentPrincipal.GetContactId();
            orderNote.Title = !string.IsNullOrEmpty(title)
                ? title
                : detail.Substring(0, Math.Min(detail.Length, 24)) + "...";
            orderNote.Detail = detail;
            orderNote.Created = DateTime.UtcNow;
            purchaseOrder.Notes.Add(orderNote);
        }
    }
}