using EPiServer.Commerce.Catalog.ContentTypes;

namespace DataMigration.Input.Model
{
    public class EpiProduct : ICmsObject
    {
        public ProductContent ProductContent { get; set; }
        public int Id => ProductContent.ContentLink.ID;
    }
}