using System;
using EPiServer.Vsf.Core.ApiBridge.Model.User;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Cart
{
    public class ShippingAddressInformation
    {
        [JsonProperty("shippingAddress")]
        public UserAddressModel ShippingAddress { get; set; }

        [JsonProperty("shippingMethodCode")]
        public string ShippingMethodCode { get; set; }

        [JsonProperty("shippingCarrierCode")]
        public string ShippingCarrierCode { get; set; }
    }
}
