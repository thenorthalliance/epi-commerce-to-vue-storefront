using System.Collections.Generic;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class OrderHistoryModel
    {
        public class SearchCriteria
        {
            public List<object> filter_groups { get; set; }
        }

        public List<object> items { get; set; } = new List<object>();
        public SearchCriteria search_criteria { get; set; } = new SearchCriteria();
    }
}
