using DataMigration.Input.Episerver.Common.Model;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace DataMigration.Input.Episerver.Product.Model
{
    public class EpiProduct : ICmsObject
    {
        public ProductContent ProductContent { get; set; }
        public int Id => ProductContent.ContentLink.ID;
    }
}