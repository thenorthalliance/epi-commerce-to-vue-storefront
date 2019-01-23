using System;
using System.Collections.Generic;

namespace EPiServer.VueStorefrontApiBridge.ApiModel
{
    public class UserModel
    {
        public string id { get; set; }
        
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string created_in { get; set; } = "default store";

        public int store_id { get; set; } = 1;
        public int website_id { get; set; } = 1;

        public int group_id { get; set; } = 1;
        public string default_shipping { get; set; } = "";

        public List<object> addresses { get; set; } = new List<object>();
        public int disable_auto_group_change { get; set; } = 0;
    }
}