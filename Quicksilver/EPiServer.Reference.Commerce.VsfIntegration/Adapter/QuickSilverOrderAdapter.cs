using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Reference.Commerce.VsfIntegration.Service;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using Mediachase.Commerce.Markets;

namespace EPiServer.Reference.Commerce.VsfIntegration.Adapter
{
    public class QuickSilverOrderAdapter : IOrderAdapter
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentManagerFacade _paymentManagerFacade;
        private readonly IMarketService _marketService;
        private readonly IEnumerable<IPaymentMethod> _paymentMethods;


        public QuickSilverOrderAdapter(
            IOrderRepository orderRepository,
            IPaymentManagerFacade paymentManagerFacade,
            IMarketService marketService,
            IEnumerable<IPaymentMethod> paymentMethods)
        {
            _orderRepository = orderRepository;
            _paymentManagerFacade = paymentManagerFacade;
            _marketService = marketService;
            _paymentMethods = paymentMethods;
        }

        public string DefaultCartName => "vsf-default-cart";

        public void CreateOrder(OrderRequestModel request)
        {
            var cart = _orderRepository.Load<ICart>(request.CartId, DefaultCartName).First();

            AddPaymentToCart(cart, request.AddressInformation.PaymentMethodCode, request.AddressInformation.BillingAddress);
            AddShippingMethod(cart, request.AddressInformation.ShippingMethodCode);
            AddShippingAddress(cart, request.AddressInformation.ShippingAddress);

            //Assign cart to user if there is one
            if (!string.IsNullOrEmpty(request.UserId))
                cart.CustomerId = Guid.Parse(request.UserId);

            _orderRepository.SaveAsPurchaseOrder(cart);
            _orderRepository.Delete(cart.OrderLink);
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
                var payment = paymentMethodImplementation.CreatePayment(cart.GetTotal().Amount, cart);

                var orderBillingAddress = cart.CreateOrderAddress("BillingAddressId");
                orderBillingAddress.City = billingAddress.City;
                orderBillingAddress.CountryCode = billingAddress.CountryId;
                orderBillingAddress.DaytimePhoneNumber = billingAddress.Telephone;
                orderBillingAddress.FirstName = billingAddress.Firstname;
                orderBillingAddress.LastName = billingAddress.Lastname;
                orderBillingAddress.PostalCode = billingAddress.Postcode;
                orderBillingAddress.Line1 = string.Join(", ", billingAddress.Street);

                payment.BillingAddress = orderBillingAddress;
                cart.AddPayment(payment);
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
                            PriceIncludingTax = lineItem.PlacedPrice, //TODO: taxes
                            QuantityOrdered = lineItem.Quantity,
                            ItemId = lineItem.LineItemId,
                            RowTotalIncludingTax = extendedPrice,
                            Sku = lineItem.Code,
                            ProductType = "configurable"
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
    }
}
