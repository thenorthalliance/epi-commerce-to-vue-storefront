using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Order;
using EPiServer.Vsf.Core.ApiBridge.Adapter;
using EPiServer.Vsf.Core.ApiBridge.Model.Order;

namespace EPiServer.Reference.Commerce.VsfIntegration.Adapter
{
    public class QuickSilverOrderAdapter : IOrderAdapter
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartAdapter _cartAdapter;
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly IPaymentProcessor _paymentProcessor;


        public QuickSilverOrderAdapter(ICartAdapter cartAdapter,
            IOrderGroupCalculator orderGroupCalculator,
            IPaymentProcessor paymentProcessor,
            IOrderRepository orderRepository)
        {
            _cartAdapter = cartAdapter;
            _cartAdapter = cartAdapter;
            _orderGroupCalculator = orderGroupCalculator;
            _paymentProcessor = paymentProcessor;
            _orderRepository = orderRepository;
        }

        public string DefaultCartName => "vsf-default-cart";

        public void CreateOrder(OrderRequestModel request)
        {
            var cart = _cartAdapter.GetCart(request.CartId);

            var shippingAddress = cart.CreateOrderAddress("ShippingAddressId");
            var billingAddress = cart.CreateOrderAddress("BillingAddressId");

            billingAddress.City = request.AddressInformation.BillingAddress.City;
            billingAddress.CountryCode = request.AddressInformation.BillingAddress.CountryId;
            billingAddress.DaytimePhoneNumber = request.AddressInformation.BillingAddress.Telephone;
            //billingAddress.Email = //take from user;
            billingAddress.FirstName = request.AddressInformation.BillingAddress.Firstname;
            billingAddress.LastName = request.AddressInformation.BillingAddress.Lastname;
            billingAddress.PostalCode = request.AddressInformation.BillingAddress.Postcode;
            billingAddress.Line1 = string.Join(", ", request.AddressInformation.BillingAddress.Street);

            var payment = cart.CreatePayment();
            payment.BillingAddress = billingAddress;

            _cartAdapter.UpdateShippingMethod(request.CartId, request.AddressInformation.ShippingCarrierCode,
                request.AddressInformation.ShippingMethodCode);


            shippingAddress.City = request.AddressInformation.ShippingAddress.City;
            shippingAddress.CountryCode = request.AddressInformation.ShippingAddress.CountryId;
            shippingAddress.DaytimePhoneNumber = request.AddressInformation.ShippingAddress.Telephone;
            //billingAddress.Email = //take from user;
            shippingAddress.FirstName = request.AddressInformation.ShippingAddress.Firstname;
            shippingAddress.LastName = request.AddressInformation.ShippingAddress.Lastname;
            shippingAddress.PostalCode = request.AddressInformation.ShippingAddress.Postcode;
            shippingAddress.Line1 = string.Join(", ", request.AddressInformation.ShippingAddress.Street);

            cart.GetFirstShipment().ShippingAddress = shippingAddress;

            //var paymentProcessingResults = cart.ProcessPayments(_paymentProcessor, _orderGroupCalculator).ToList();

            //if (paymentProcessingResults.Any(r => !r.IsSuccessful))
            //{
            //    modelState.AddModelError("", _localizationService.GetString("/Checkout/Payment/Errors/ProcessingPaymentFailure") + string.Join(", ", paymentProcessingResults.Select(p => p.Message)));
            //    return null;
            //}

            //var redirectPayment = paymentProcessingResults.FirstOrDefault(r => !string.IsNullOrEmpty(r.RedirectUrl));
            //if (redirectPayment != null)
            //{
            //    checkoutViewModel.RedirectUrl = redirectPayment.RedirectUrl;
            //    return null;
            //}

            //var processedPayments = cart.GetFirstForm().Payments.Where(x => x.Status.Equals(PaymentStatus.Processed.ToString())).ToList();
            //if (!processedPayments.Any())
            //{
            //    // Return null in case there is no payment was processed.
            //    return null;
            //}

            //var totalProcessedAmount = processedPayments.Sum(x => x.Amount);
            //if (totalProcessedAmount != cart.GetTotal(_orderGroupCalculator).Amount)
            //{
            //    throw new InvalidOperationException("Wrong amount");
            //}

            //PurchaseValidation validation;
            //if (checkoutViewModel.IsAuthenticated)
            //{
            //    validation = AuthenticatedPurchaseValidation;
            //}
            //else
            //{
            //    validation = AnonymousPurchaseValidation;
            //}

            //if (!validation.ValidateOrderOperation(modelState, _cartService.RequestInventory(cart)))
            //{
            //    return null;
            //}

            //Assign cart to user if there is one
            if (!string.IsNullOrEmpty(request.UserId))
                cart.CustomerId = Guid.Parse(request.UserId);

            var orderReference = _orderRepository.SaveAsPurchaseOrder(cart);
            _orderRepository.Delete(cart.OrderLink);
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
                var billingAddress = purchaseOrder.GetFirstForm().Payments.FirstOrDefault()?.BillingAddress;
                var shipment = purchaseOrder.GetFirstShipment();

                var orderDetails = new OrderDetails()
                {
                    OrderNumber = orderForm.OrderFormId,
                    CreatedAt = purchaseOrder.Created,
                    CustomerFirstname = billingAddress?.FirstName ?? "Test",
                    CustomerLastname = billingAddress?.LastName ?? "Was",
                    Status = purchaseOrder.OrderStatus.ToString(),
                    GrandTotal = purchaseOrder.GetTotal().Amount,
                    Subtotal = purchaseOrder.GetSubTotal().Amount,
                    ShippingAmount = purchaseOrder.GetShippingTotal().Amount,
                    TaxAmount = purchaseOrder.GetTaxTotal().Amount,
                    OrderedProducts = new List<OrderProductDetails>(),
                    ShippingDescription = shipment.ShippingMethodName,
                    Payment = new OrderPayment
                    {
                        AdditionalInformation = new List<string>()
                        {
                            "Credit Card Direct Post (Authorize.net)"
                        }
                    },
                    BillingAddress = new OrderAddress
                    {
                        AddressType = "billing",
                        City = "California",
                        CountryId = "US",
                        Email = "test2@test.pl",
                        EntityId = "8361",
                        Firstname = "test",
                        Lastname = "test",
                        ParentOrderId = orderForm.OrderFormId,
                        Postcode = "81120",
                        Street = new List<string>()
                        {
                            "Test",
                            "34"
                        },
                        Telephone = "1234567890"
                    },
                    ExtensionAttributes = new ExtensionAttributes()
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
                            RowTotalIncludingTax = extendedPrice, //probably incorrect if item quantity can be 1.3
                            Sku = lineItem.Code
                        });
                }

                // show full discount, so add order discount + per item discounts + shipping discount if applicable
                orderDetails.DiscountAmount = purchaseOrder.GetOrderDiscountTotal().Amount
                                              + purchaseOrder.GetShippingDiscountTotal().Amount;

                orderHistoryModel.Orders.Add(orderDetails);
            }

            return orderHistoryModel;
        }
    }
}
