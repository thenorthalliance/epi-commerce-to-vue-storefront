using AutoMapper;
using EPiServer.Vsf.Core.Models.PayPal.Payments;
using PayPalCheckoutSdk.Orders;

namespace EPiServer.Vsf.Core.AutoMapper
{
    public class PayPalDomainMappingProfile : Profile
    {
        public PayPalDomainMappingProfile()
        {
            //Client models to domain models
            CreateMap<Order, Models.PayPal.Orders.Order>()
                .ReverseMap();

            CreateMap<AddressDetails, Models.PayPal.Orders.AddressDetails>();
            CreateMap<AddressPortable, Models.PayPal.Orders.AddressPortable>();
            CreateMap<AmountWithBreakdown, Models.PayPal.Orders.AmountWithBreakdown>();
            CreateMap<Authorization, Models.PayPal.Orders.Authorization>();
            CreateMap<Customer, Models.PayPal.Orders.Customer>();
            CreateMap<DisplayableMerchantInformation, Models.PayPal.Orders.DisplayableMerchantInformation>();
            CreateMap<Item, Models.PayPal.Orders.Item>();
            CreateMap<LinkDescription, Models.PayPal.Orders.LinkDescription>();
            CreateMap<MerchantBase, Models.PayPal.Orders.MerchantBase>();
            CreateMap<Money, Models.PayPal.Orders.Money>();
            CreateMap<Name, Models.PayPal.Orders.Name>();
            CreateMap<Order, Models.PayPal.Orders.Order>();
            CreateMap<Payee, Models.PayPal.Orders.Payee>();
            CreateMap<PaymentCollection, Models.PayPal.Orders.PaymentCollection>();
            CreateMap<PaymentInstruction, Models.PayPal.Orders.PaymentInstruction>();
            CreateMap<Phone, Models.PayPal.Orders.Phone>();
            CreateMap<PlatformFee, Models.PayPal.Orders.PlatformFee>();
            CreateMap<PurchaseUnit, Models.PayPal.Orders.PurchaseUnit>();
            CreateMap<SellerProtection, Models.PayPal.Orders.SellerProtection>();
            CreateMap<ShippingDetails, Models.PayPal.Orders.ShippingDetails>();
            CreateMap<TaxInformation, Models.PayPal.Orders.TaxInformation>();
            CreateMap<Capture, Models.PayPal.Orders.Capture>();
            CreateMap<Refund, Models.PayPal.Orders.Refund>();
            CreateMap<MerchantPayableBreakdown, Models.PayPal.Orders.MerchantPayableBreakdown>();
            CreateMap<MerchantReceivableBreakdown, Models.PayPal.Orders.MerchantReceivableBreakdown>();
            CreateMap<ExchangeRate, Models.PayPal.Orders.ExchangeRate>();
            CreateMap<NetAmountBreakdownItem, Models.PayPal.Orders.NetAmountBreakdownItem>();
            CreateMap<StatusDetails, Models.PayPal.Orders.StatusDetails>();

            CreateMap<PayPalCheckoutSdk.Payments.Capture, PayPalCapture>()
                .ForMember(m => m.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(m => m.Status, opt => opt.MapFrom(s => s.Status));
        }
    }
}
