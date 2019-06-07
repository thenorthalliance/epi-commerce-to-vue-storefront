using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.VsfIntegration.Service;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using EPiServer.Vsf.Core.Models.PayPal;
using EPiServer.Vsf.Core.Payments;
using EPiServer.Vsf.Core.Services;
using Mediachase.Commerce.Markets;
using Order = EPiServer.Vsf.Core.Models.PayPal.Orders.Order;

namespace EPiServer.Reference.Commerce.VsfIntegration.Adapter
{
    public class QuickSilverOrderAdapter : IOrderAdapter
    {
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentManagerFacade _paymentManagerFacade;
        private readonly IMarketService _marketService;
        private readonly IEnumerable<IPaymentMethod> _paymentMethods;
        private readonly IPayPalService _payPalService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;

        public QuickSilverOrderAdapter(
            IPaymentProcessor paymentProcessor,
            IOrderRepository orderRepository,
            IPaymentManagerFacade paymentManagerFacade,
            IMarketService marketService,
            IEnumerable<IPaymentMethod> paymentMethods,
            IPayPalService payPalService,
            IOrderGroupCalculator orderGroupCalculator)
        {
            _paymentProcessor = paymentProcessor;
            _orderRepository = orderRepository;
            _paymentManagerFacade = paymentManagerFacade;
            _marketService = marketService;
            _paymentMethods = paymentMethods;
            _payPalService = payPalService;
            _orderGroupCalculator = orderGroupCalculator;
        }

        public string DefaultCartName => "vsf-default-cart";

        public OrderResponseModel CreateOrder(OrderRequestModel request)
        {
            var cart = _orderRepository.Load<ICart>(request.CartId, DefaultCartName).First();

            AddPaymentToCart(cart, request.AddressInformation.PaymentMethodCode, request.AddressInformation.BillingAddress);
            AddShippingMethod(cart, request.AddressInformation.ShippingMethodCode);
            AddShippingAddress(cart, request.AddressInformation.ShippingAddress);

            //Assign cart to user if there is one
            if (!string.IsNullOrEmpty(request.UserId))
                cart.CustomerId = Guid.Parse(request.UserId);

            var savedOrderReference = _orderRepository.SaveAsPurchaseOrder(cart);
            var savedOrderId = savedOrderReference.OrderGroupId;

            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(savedOrderReference.OrderGroupId);
            purchaseOrder.ProcessPayments(_paymentProcessor, _orderGroupCalculator);

            _orderRepository.Delete(cart.OrderLink);
            var newCart = _orderRepository.Create<ICart>(request.CartId, DefaultCartName);
            _orderRepository.Save(newCart);

            return new OrderResponseModel
            {
                OrderId = savedOrderId
            };
        }

        private void AddShippingAddress(ICart cart, UserAddressModel shippingAddress)
        {
            var orderShippingAddress = cart.CreateOrderAddress("ShippingAddressId");

            orderShippingAddress.City = shippingAddress.City;
            orderShippingAddress.CountryCode = shippingAddress.CountryId;
            orderShippingAddress.DaytimePhoneNumber = shippingAddress.Telephone;
            orderShippingAddress.FirstName = shippingAddress.Firstname;
            orderShippingAddress.LastName = shippingAddress.Lastname;
            orderShippingAddress.PostalCode = shippingAddress.Postcode;
            orderShippingAddress.Line1 = string.Join(", ", shippingAddress.Street);

            cart.GetFirstShipment().ShippingAddress = orderShippingAddress;
        }

        private void AddShippingMethod(ICart cart, Guid shippingMethodId)
        {
            var shipment = cart.GetFirstShipment();
            shipment.ShippingMethodId = shippingMethodId;
        }

        private void AddPaymentToCart(ICart cart, Guid paymentMethodGuid, UserAddressModel billingAddress)
        {
            var market = _marketService.GetMarket(cart.MarketId);

            var paymentMethod = _paymentManagerFacade
                .GetPaymentMethodsByMarket(market.MarketId.Value, market.DefaultLanguage.TwoLetterISOLanguageName)
                .PaymentMethod
                .FindByPaymentMethodId(paymentMethodGuid);

            var paymentMethodImplementation = _paymentMethods.SingleOrDefault(x => x.PaymentMethodId == paymentMethod.PaymentMethodId);

            if (paymentMethodImplementation != null)
            {
                var orderBillingAddress = cart.CreateOrderAddress("BillingAddressId");
                orderBillingAddress.City = billingAddress.City;
                orderBillingAddress.CountryCode = billingAddress.CountryId;
                orderBillingAddress.DaytimePhoneNumber = billingAddress.Telephone;
                orderBillingAddress.FirstName = billingAddress.Firstname;
                orderBillingAddress.LastName = billingAddress.Lastname;
                orderBillingAddress.PostalCode = billingAddress.Postcode;
                orderBillingAddress.Line1 = string.Join(", ", billingAddress.Street);

                var existingPayment = cart.GetFirstForm().Payments.FirstOrDefault(x => x.PaymentMethodId == paymentMethodGuid);
                if (existingPayment != null)
                {
                    existingPayment.BillingAddress = orderBillingAddress;
                }
                else
                {
                    var payment = paymentMethodImplementation.CreatePayment(cart.GetTotal().Amount, cart);
                    payment.BillingAddress = orderBillingAddress;
                    cart.AddPayment(payment);
                }
            }
        }

        public OrderHistoryModel GetOrders(string userId)
        {
            if(string.IsNullOrEmpty(userId))
                throw new ArgumentNullException($"{nameof(userId)} not provided.");

            if (!Guid.TryParse(userId, out var customerId))
                throw new ArgumentOutOfRangeException($"{nameof(userId)} is not a guid.");

            var purchaseOrders = _orderRepository.Load<IPurchaseOrder>(customerId, DefaultCartName);
            var orderHistoryModel = new OrderHistoryModel();
            foreach (var purchaseOrder in purchaseOrders)
            {
                var orderForm = purchaseOrder.GetFirstForm();
                var payment = purchaseOrder.GetFirstForm().Payments.FirstOrDefault();
                var billingAddress = payment?.BillingAddress;
                var shipment = purchaseOrder.GetFirstShipment();

                var orderDetails = new OrderDetails()
                {
                    OrderNumber = orderForm.OrderFormId,
                    CreatedAt = purchaseOrder.Created,
                    CustomerFirstname = billingAddress?.FirstName,
                    CustomerLastname = billingAddress?.LastName,
                    Status = purchaseOrder.OrderStatus.ToString(),
                    GrandTotal = purchaseOrder.GetTotal().Amount,
                    Subtotal = purchaseOrder.GetSubTotal().Amount,
                    ShippingAmount = purchaseOrder.GetShippingTotal().Amount,
                    TaxAmount = purchaseOrder.GetTaxTotal().Amount,
                    OrderedProducts = new List<OrderProductDetails>(),
                    ShippingDescription = shipment.ShippingMethodName,
                    BillingAddress = new OrderAddress
                    {
                        AddressType = "billing",
                        City = billingAddress?.City,
                        CountryId = billingAddress?.CountryCode,
                        Email = billingAddress?.Email,
                        EntityId = billingAddress?.Id,
                        Firstname = billingAddress?.FirstName,
                        Lastname = billingAddress?.LastName,
                        ParentOrderId = orderForm.OrderFormId,
                        Postcode = billingAddress?.PostalCode,
                        Street = new List<string>()
                        {
                            billingAddress?.Line1,
                            billingAddress?.Line2
                        },
                        Telephone = billingAddress?.DaytimePhoneNumber
                    },
                    ExtensionAttributes = new OrderExtensionAttributes()
                    {
                        ShippingAssignments = new List<OrderShippingAssignments>()
                        {
                            new OrderShippingAssignments()
                            {
                                Shipping = new OrderShipping()
                                {
                                    Address = new OrderAddress()
                                    {
                                        AddressType = "shipping",
                                        City = shipment.ShippingAddress.City,
                                        Company = shipment.ShippingAddress.Organization,
                                        CountryId = shipment.ShippingAddress.CountryCode,
                                        Email = shipment.ShippingAddress.Email,
                                        EntityId = shipment.ShippingAddress.Id,
                                        Firstname = shipment.ShippingAddress.FirstName,
                                        Lastname = shipment.ShippingAddress.LastName,
                                        ParentOrderId = orderForm.OrderFormId,
                                        Postcode = shipment.ShippingAddress.PostalCode,
                                        Street = new List<string>()
                                        {
                                            shipment.ShippingAddress.Line1,
                                            shipment.ShippingAddress.Line2
                                        },
                                        Telephone = shipment.ShippingAddress.DaytimePhoneNumber
                                    }
                                }
                            }
                        }
                    }
                };

                var lineItems = purchaseOrder.GetAllLineItems();

                foreach (var lineItem in lineItems)
                {
                    var extendedPrice = lineItem.GetExtendedPrice(purchaseOrder.Currency);

                    orderDetails.OrderedProducts.Add(
                        new OrderProductDetails
                        {
                            Name = lineItem.DisplayName,
                            OrderId = orderForm.OrderFormId,
                            Price = lineItem.PlacedPrice,
                            PriceIncludingTax = lineItem.PlacedPrice,
                            QuantityOrdered = lineItem.Quantity,
                            ItemId = lineItem.LineItemId,
                            RowTotalIncludingTax = extendedPrice,
                            Sku = lineItem.Code,
                            ProductType = "simple"
                        });
                }

                orderDetails.DiscountAmount = 
                    purchaseOrder.GetOrderDiscountTotal().Amount + 
                    purchaseOrder.GetShippingDiscountTotal().Amount;

                if (payment != null)
                {
                    var paymentName = _paymentMethods.FirstOrDefault(x => x.SystemKeyword == payment.PaymentMethodName)?.Name;
                    if (paymentName != null)
                    {
                        orderDetails.Payment = new OrderPayment
                        {
                            AdditionalInformation = new List<string>()
                            {
                                paymentName
                            }
                        };
                    }
                }

                orderHistoryModel.Orders.Add(orderDetails);
            }

            return orderHistoryModel;
        }

        public async Task<Order> CreatePaypalOrder(PayPalCreateOrder request)
        {
            var cart = _orderRepository.Load<ICart>(request.CartId, DefaultCartName).First();
            var shipment = cart.GetFirstShipment();
            var shippingAddress = shipment.ShippingAddress;

            if (shippingAddress == null)
                throw new Exception("ShippingAddress must be set on current cart");

            request.ShippingData = new PayPalShippingData
            {
                City = shippingAddress.City,
                CountryCode = shippingAddress.CountryCode,
                PostalCode = shippingAddress.PostalCode
            };

            var order = await _payPalService.CreateOrderAsync(request).ConfigureAwait(false);

            _orderRepository.Save(cart);

            return order;
        }

        public async Task<Order> AuthorizePaypalOrder(PayPalCaptureRequest request)
        {
            var cart = _orderRepository.Load<ICart>(request.CartId, DefaultCartName).First();

            var market = _marketService.GetMarket(cart.MarketId);

            var paymentMethod = _paymentManagerFacade
                .GetPaymentMethodsByMarket(market.MarketId.Value, market.DefaultLanguage.TwoLetterISOLanguageName)
                .PaymentMethod
                .First(p => p.SystemKeyword == PayPalConfiguration.PayPalSystemName);

            var paymentMethodImplementation = _paymentMethods.Single(x => x.PaymentMethodId == paymentMethod.PaymentMethodId);
            var payment = paymentMethodImplementation.CreatePayment(cart.GetTotal().Amount, cart);

            var order = await _payPalService.AuthorizeOrderAsync(request.OrderId).ConfigureAwait(false);

            payment.Properties[PayPalConfiguration.PayPalOrderNumber] = order.Id;
            payment.Properties[PayPalConfiguration.PayPalExpToken] = order.PurchaseUnits.First().Payments.Authorizations.First().Id; //needed for capture later on
            payment.TransactionType = "Capture";

            cart.GetFirstForm().Payments.Clear();

            cart.AddPayment(payment);
            _orderRepository.Save(cart);

            return order;
        }
    }
}
