using EPiServer.Commerce.Catalog.ContentTypes;

namespace EPiServer.Vsf.DataExport.Input.Model
{
    public class EpiProduct : ICmsObject
    {
        public ProductContent ProductContent { get; set; }
        public int Id => ProductContent.ContentLink.ID;
    }
}