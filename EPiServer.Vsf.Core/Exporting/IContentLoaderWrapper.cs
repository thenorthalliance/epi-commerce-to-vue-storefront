using System.Collections.Generic;
using System.Globalization;
using EPiServer.Core;

namespace EPiServer.Vsf.Core.Exporting
{
    public interface IContentLoaderWrapper
    {
        IEnumerable<PropertyData> GetVariantVsfProperties(ContentReference variantReference);

        T Get<T>(ContentReference reference) where T : IContentData;

        IEnumerable<T> GetChildren<T>(ContentReference contentLink, CultureInfo language, int startIndex, int maxRows) where T : IContentData;

        IEnumerable<T> GetChildren<T>(ContentReference contentLink) where T : IContentData;
    }
}