using System.Collections.Generic;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace EPiServer.Vsf.DataExport.Input.Model
{
    public class EpiCategory : ICmsObject
    {
        public NodeContent Category { get; set; }
        public IEnumerable<EpiCategory> Children { get; set; }
        public int Level { get; set; }
        public int SortOrder { get; set; }
        public int ProductsCount { get; set; }
        public int Id => Category.ContentLink.ID;
    }
}