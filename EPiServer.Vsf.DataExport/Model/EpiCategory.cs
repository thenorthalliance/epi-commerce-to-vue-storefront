using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace EPiServer.Vsf.DataExport.Model
{
    public class EpiCategory
    {
        public NodeContent Category { get; set; }
        public IEnumerable<EpiCategory> Children { get; set; }
        public int Level { get; set; }
        public int SortOrder { get; set; }
        public int TotalProductsCount => Children.Select(x => x.TotalProductsCount).DefaultIfEmpty(0).Sum() + CategoryProductsCount;
        public int CategoryProductsCount { get; set; }
    }
}