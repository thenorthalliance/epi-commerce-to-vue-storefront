using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.Vsf.Core.ApiBridge.Model.Order
{
    public class OrderHistoryModel
    {
        public class SearchCriteriaModel
        {
            [JsonProperty("filter_groups")]
            public List<object> FilterGroups { get; set; }
        }

        [JsonProperty("items")] //orders
        public List<OrderDetails> Orders { get; set; } = new List<OrderDetails>();

        [JsonProperty("search_criteria")]
        public SearchCriteriaModel SearchCriteria { get; set; } = new SearchCriteriaModel();
    }
}
