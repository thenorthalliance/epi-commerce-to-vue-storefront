using System.Collections.Generic;
using Newtonsoft.Json;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class OrderHistoryModel
    {
        public class SearchCriteriaModel
        {
            [JsonProperty("filter_groups")]
            public List<object> FilterGroups { get; set; }
        }

        [JsonProperty("items")]
        public List<object> Items { get; set; } = new List<object>();

        [JsonProperty("search_criteria")]
        public SearchCriteriaModel SearchCriteria { get; set; } = new SearchCriteriaModel();
    }
}
