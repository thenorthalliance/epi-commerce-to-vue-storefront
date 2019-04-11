using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Vsf.DataExport.Attributes;
using Mediachase.Commerce.Catalog;

namespace EPiServer.Vsf.DataExport.Utils.Epi
{
    public class ContentService
    {
        private readonly IContentLoader _contentLoader;
        private readonly ReferenceConverter _referenceConverter;

        public ContentService(IContentLoader contentLoader, ReferenceConverter referenceConverter)
        {
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter;
        }

        public T GetContent<T>(ContentReference contentReference) where T : IContent
        {
            return _contentLoader.Get<T>(contentReference);
        }

        public IEnumerable<CatalogContent> GetRootCatalogs()
        {
            return _contentLoader.GetChildren<CatalogContent>(_referenceConverter.GetRootLink());
        }

        public IEnumerable<PropertyData> GetVariantVsfProperties(ContentReference variantReference)
        {
            var variant = GetContent<VariationContent>(variantReference);
            var propertiesNames = variant.GetType().GetProperties().Where(x => System.Attribute.IsDefined(x, typeof(VsfOptionAttribute))).Select(x => x.Name);
            return variant.Property.Where(x => propertiesNames.Contains(x.Name));
        }

        public IEnumerable<T> LoadChildrenBatched<T>(ContentReference parentLink, CultureInfo defaultCulture) where T : IContent
        {
            var start = 0;
            while (true)
            {
                var batch = _contentLoader.GetChildren<T>(parentLink, defaultCulture, start, 50);
                var enumerable = batch.ToList();
                if (!enumerable.Any())
                {
                    yield break;
                }

                foreach (var content in enumerable)
                {
                    if (!parentLink.CompareToIgnoreWorkID(content.ParentLink))
                    {
                        continue;
                    }

                    yield return content;
                }

                start += 50;
            }
        }
    }
}
