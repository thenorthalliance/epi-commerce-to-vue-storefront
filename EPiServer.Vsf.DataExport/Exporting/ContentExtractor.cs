using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Globalization;
using EPiServer.Vsf.Core.Exporting;
using Mediachase.Commerce.Catalog;

namespace EPiServer.Vsf.DataExport.Exporting
{
    public class ContentExtractor : IContentExtractor
    {
        private const int MaxBatshSize = 50;
        
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;
        private readonly CultureInfo _cultureInfo = ContentLanguage.PreferredCulture;

        public ContentExtractor(IContentLoader contentLoader, ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
        }

        public void Extract(IExtractedContentHandler contentHandler)
        {
            contentHandler.OnBeginExtraction();

            var rootCatalog = GetRootCatalog(_contentLoader, _referenceConverter);
            ExtractNode(rootCatalog, contentHandler);
            contentHandler.OnFinishExtraction();
        }

        private void ExtractNode(NodeContentBase parentNode, IExtractedContentHandler contentHandler)
        {
            if(parentNode == null)
                return;

            foreach (var child in BatchedChildren<NodeContent>(parentNode.ContentLink))
            {
                contentHandler.OnNodeContent(child, parentNode);
                ExtractNode(child, contentHandler);
                ExtractProducts(child, contentHandler);
            }
        }

        private void ExtractProducts(NodeContent parent, IExtractedContentHandler contentHandler)
        {
            foreach (var product in BatchedChildren<ProductContent>(parent.ContentLink))
            {
                contentHandler.OnProductContent(parent, product);
            }
        }

        private IEnumerable<T> BatchedChildren<T>(ContentReference parentLink) where T : IContent
        {
            var start = 0;
            while (true)
            {
                var batch = _contentLoader.GetChildren<T>(parentLink, _cultureInfo, start, MaxBatshSize);
                var enumerable = batch.ToList();

                if (!enumerable.Any())
                    yield break;

                foreach (var content in enumerable)
                {
                    if (!parentLink.CompareToIgnoreWorkID(content.ParentLink))
                        continue;

                    yield return content;
                }

                start += 50;
            }
        }

        private static CatalogContent GetRootCatalog(IContentLoader contentLoader, ReferenceConverter referenceConverter)
        {
            return contentLoader.GetChildren<CatalogContent>(referenceConverter.GetRootLink())?.FirstOrDefault();
        }
    }
}