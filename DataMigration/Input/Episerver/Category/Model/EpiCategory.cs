using System.Collections.Generic;
using DataMigration.Input.Episerver.Common.Model;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace DataMigration.Input.Episerver.Category.Model
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