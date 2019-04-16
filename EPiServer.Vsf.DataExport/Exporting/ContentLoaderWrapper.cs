using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Vsf.Core.Exporting;
using EPiServer.Vsf.DataExport.Attributes;

namespace EPiServer.Vsf.DataExport.Exporting
{
    public class ContentLoaderWrapper : IContentLoaderWrapper
    {
        private readonly IContentLoader _contentLoader;

        public ContentLoaderWrapper(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public IEnumerable<PropertyData> GetVariantVsfProperties(ContentReference variantReference)
        {
            var variant = _contentLoader.Get<VariationContent>(variantReference);

            var propertiesNames = variant.GetType().GetProperties()
                .Where(x => System.Attribute.IsDefined(x, typeof(VsfOptionAttribute))).Select(x => x.Name);

            var variantVsfProperties = variant.Property.Where(x => propertiesNames.Contains(x.Name))
                .ToList();

            return variantVsfProperties;
        }

        public T Get<T>(ContentReference reference) where T : IContentData
        {
            return _contentLoader.Get<T>(reference);
        }

        public IEnumerable<T> GetChildren<T>(ContentReference contentLink, CultureInfo language, int startIndex, int maxRows) where T : IContentData
        {
            return _contentLoader.GetChildren<T>(contentLink, language, startIndex, maxRows);
        }

        public IEnumerable<T> GetChildren<T>(ContentReference contentLink) where T : IContentData
        {
            return _contentLoader.GetChildren<T>(contentLink);
        }
    }
}