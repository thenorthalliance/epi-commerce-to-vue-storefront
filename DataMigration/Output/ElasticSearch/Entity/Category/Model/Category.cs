using System.Collections.Generic;
using System.Linq;
using DataMigration.Input.Episerver.Category.Model;
using EPiServer.Core;
using Nest;
using Newtonsoft.Json;

namespace DataMigration.Output.ElasticSearch.Entity.Category.Model
{
    public class Category : CategoryBase
    {
        
        [PropertyName("description")]
        public string Description { get; set; }

        [PropertyName("is_active")]
        public bool IsActive { get; set; }

        //1 is a root category
        [PropertyName("level")]
        public int Level { get; set; }

        [PropertyName("product_count")]
        public int ProductCount { get; set; }

        [PropertyName("available_sort_by")]
        public IEnumerable<string> AvailableSortBy { get; set; }
    }
}

