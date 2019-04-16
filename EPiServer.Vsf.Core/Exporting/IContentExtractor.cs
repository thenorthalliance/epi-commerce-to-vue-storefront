namespace EPiServer.Vsf.Core.Exporting
{
    public interface IContentExtractor
    {
        void Extract(IExtractedContentHandler contentHandler);
    }
}
