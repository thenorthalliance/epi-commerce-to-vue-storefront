using EPiServer.Commerce.Catalog.ContentTypes;

namespace EPiServer.Vsf.Core.Exporting
{
    public interface IExtractedContentHandler
    {
        void OnBeginExtraction();

        void OnNodeContent(NodeContent content, NodeContentBase parentNode);
        void OnProductContent(NodeContent parent, ProductContent product);
        
        void OnFinishExtraction();
    }
}