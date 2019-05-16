using System;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderAddressInformation
    {
        [JsonProperty("shippingAddress")]
        public UserAddressModel ShippingAddress { get; set; }

        [JsonProperty("billingAddress")]
        public UserAddressModel BillingAddress { get; set; }

        [JsonProperty("shipping_method_code")]
        public Guid ShippingMethodCode { get; set; }

        [JsonProperty("shipping_carrier_code")]
        public string ShippingCarrierCode { get; set; }

        [JsonProperty("payment_method_code")]
        public Guid PaymentMethodCode { get; set; }

        [JsonProperty("payment_method_additional")]
        public object PaymentMethodAdditional { get; set; }
    }
}
