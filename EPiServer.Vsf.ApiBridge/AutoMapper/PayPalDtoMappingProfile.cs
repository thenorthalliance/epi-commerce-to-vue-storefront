using AutoMapper;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Requests;
using EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal.Responses;

namespace EPiServer.Vsf.ApiBridge.AutoMapper
{
    public class PayPalDtoMappingProfile : Profile
    {
        public PayPalDtoMappingProfile()
        {
            //Domain models to api models
            CreateMap<Core.Models.PayPal.Orders.Order, Order>();
            CreateMap<Core.Models.PayPal.Orders.Order, PayPalOrderResponse>()
                .ForMember(payPalOrder => payPalOrder.OrderId, opt => opt.MapFrom(order => order.Id));

            CreateMap<Core.Models.PayPal.Orders.AddressDetails, AddressDetails>();
            CreateMap<Core.Models.PayPal.Orders.AddressPortable, AddressPortable>();
            CreateMap<Core.Models.PayPal.Orders.AmountWithBreakdown, AmountWithBreakdown>();
            CreateMap<Core.Models.PayPal.Orders.Authorization, Core.ApiBridge.Model.Order.PayPal.Authorization>();
            CreateMap<Core.Models.PayPal.Orders.Customer, Customer>();
            CreateMap<Core.Models.PayPal.Orders.DisplayableMerchantInformation, DisplayableMerchantInformation>();
            CreateMap<Core.Models.PayPal.Orders.Item, Item>();
            CreateMap<Core.Models.PayPal.Orders.LinkDescription, LinkDescription>();
            CreateMap<Core.Models.PayPal.Orders.MerchantBase, MerchantBase>();
            CreateMap<Core.Models.PayPal.Orders.Money, Money>();
            CreateMap<Core.Models.PayPal.Orders.Name, Name>();
            CreateMap<Core.Models.PayPal.Orders.Payee, Payee>();
            CreateMap<Core.Models.PayPal.Orders.PaymentCollection, PaymentCollection>();
            CreateMap<Core.Models.PayPal.Orders.PaymentInstruction, PaymentInstruction>();
            CreateMap<Core.Models.PayPal.Orders.Phone, Phone>();
            CreateMap<Core.Models.PayPal.Orders.PlatformFee, PlatformFee>();
            CreateMap<Core.Models.PayPal.Orders.PurchaseUnit, PurchaseUnit>();
            CreateMap<Core.Models.PayPal.Orders.SellerProtection, SellerProtection>();
            CreateMap<Core.Models.PayPal.Orders.ShippingDetails, ShippingDetails>();
            CreateMap<Core.Models.PayPal.Orders.TaxInformation, TaxInformation>();
            CreateMap<Core.Models.PayPal.Orders.Capture, Capture>();
            CreateMap<Core.Models.PayPal.Orders.Refund, Refund>();
            CreateMap<Core.Models.PayPal.Orders.MerchantPayableBreakdown, MerchantPayableBreakdown>();
            CreateMap<Core.Models.PayPal.Orders.MerchantReceivableBreakdown, MerchantReceivableBreakdown>();
            CreateMap<Core.Models.PayPal.Orders.ExchangeRate, ExchangeRate>();
            CreateMap<Core.Models.PayPal.Orders.NetAmountBreakdownItem, NetAmountBreakdownItem>();
            CreateMap<Core.Models.PayPal.Orders.StatusDetails, StatusDetails>();

            //requests
            CreateMap<PayPalCaptureRequest, Core.Models.PayPal.PayPalCaptureRequest>();
            CreateMap<PayPalCreateOrderRequest, Core.Models.PayPal.PayPalCreateOrder>();
            CreateMap<PayPalTransaction, Core.Models.PayPal.PayPalTransaction>();
            CreateMap<PayPalTransactionDetails, Core.Models.PayPal.PayPalTransactionDetails>();
        }
    }
}
