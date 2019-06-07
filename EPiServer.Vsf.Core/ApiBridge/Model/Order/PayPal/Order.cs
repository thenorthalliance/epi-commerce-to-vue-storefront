using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order.PayPal
{
    public class Order
    {
        [JsonProperty("createTime")]
        public string CreateTime { get; set; }

        [JsonProperty("createTime")]
        public string Id { get; set; }

        [JsonProperty("createTime")]
        public string Intent { get; set; }

        [JsonProperty("createTime")]
        public List<LinkDescription> Links { get; set; }

        [JsonProperty("createTime")]
        public Customer Payer { get; set; }

        [JsonProperty("createTime")]
        public List<PurchaseUnit> PurchaseUnits { get; set; }

        [JsonProperty("createTime")]
        public string Status { get; set; }

        [JsonProperty("createTime")]
        public string UpdateTime { get; set; }
    }
}
