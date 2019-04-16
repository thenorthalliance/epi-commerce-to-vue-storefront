using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Caching;
using EPiServer.Core;
using EPiServer.Vsf.Core.Exporting;

namespace EPiServer.Vsf.DataExport.Exporting
{
    public class CachedContentLoaderWrapper : IContentLoaderWrapper
    {
        private static readonly MemoryCache VariantsCache = MemoryCache.Default;

        private readonly IContentLoaderWrapper _contentLoaderWrapper;

        public CachedContentLoaderWrapper(IContentLoaderWrapper contentLoaderWrapper)
        {
            _contentLoaderWrapper = contentLoaderWrapper;
        }

        public IEnumerable<PropertyData> GetVariantVsfProperties(ContentReference variantReference)
        {
            var key = variantReference.ID.ToString();

            var cachedVariants = (IEnumerable<PropertyData>)VariantsCache.Get(key);

            if (cachedVariants != null)
                return cachedVariants;
            
            var variantVsfProperties = _contentLoaderWrapper.GetVariantVsfProperties(variantReference);

            VariantsCache.Add(new CacheItem(key, variantVsfProperties), new CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromSeconds(60)
            });

            return variantVsfProperties;            
        }

        public T Get<T>(ContentReference reference) where T : IContentData
        {
            return _contentLoaderWrapper.Get<T>(reference);
        }

        public IEnumerable<T> GetChildren<T>(ContentReference contentLink, CultureInfo language, int startIndex, int maxRows) where T : IContentData
        {
            return _contentLoaderWrapper.GetChildren<T>(contentLink, language, startIndex, maxRows);
        }

        public IEnumerable<T> GetChildren<T>(ContentReference contentLink) where T : IContentData
        {
            return _contentLoaderWrapper.GetChildren<T>(contentLink);
        }
    }
}