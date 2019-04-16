using System.Collections.Generic;
using System.Linq;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Vsf.DataExport.Attributes;

namespace EPiServer.Vsf.DataExport.Utils.Epi
{
    public static class VariantLoaderHelper
    {
        public static IEnumerable<PropertyData> GetVariantVsfProperties(this IContentLoader contentLoader, ContentReference variantReference)
        {
            var variant = contentLoader.Get<VariationContent>(variantReference);
            var propertiesNames = variant.GetType().GetProperties().Where(x => System.Attribute.IsDefined(x, typeof(VsfOptionAttribute))).Select(x => x.Name);
            return variant.Property.Where(x => propertiesNames.Contains(x.Name));
        }
    }
}
