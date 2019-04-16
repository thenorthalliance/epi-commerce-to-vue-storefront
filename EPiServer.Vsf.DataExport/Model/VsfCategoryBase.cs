using System.Collections.Generic;
using Nest;

namespace EPiServer.Vsf.DataExport.Model
{
    public class VsfCategoryBase
    {
        [PropertyName("id")]
        public int Id { get; set; }

        [PropertyName("name")]
        public string Name { get; set; }

        [PropertyName("parent_id")]
        public int ParentId { get; set; }
        
        [PropertyName("position")]
        public int Position { get; set; }

        [PropertyName("children_count")]
        public string ChildrenCount { get; set; }

        [PropertyName("children_data")]
        public IEnumerable<VsfCategoryBase> Children { get; set; }

        [PropertyName("include_in_menu")]
        public bool IncludeInMenu { get; set; }

        [Keyword(Name = "url_key")]
        public string UrlKey { get; set; }

    }
}