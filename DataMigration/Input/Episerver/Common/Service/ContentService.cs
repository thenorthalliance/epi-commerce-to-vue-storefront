using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Common.Model;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog;

namespace DataMigration.Input.Episerver.Common.Service
{
    public abstract class ContentService
    {
        private readonly IContentLoader _contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        private readonly ReferenceConverter _referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();

        public abstract IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level); 

        internal IEnumerable<T> GetEntriesRecursive<T>(ContentReference parentLink, CultureInfo defaultCulture) where T : IContent
        {
            foreach (var nodeContent in LoadChildrenBatched<T>(parentLink, defaultCulture))
            {
                foreach (var entry in GetEntriesRecursive<T>(nodeContent.ContentLink, defaultCulture))
                {
                    yield return entry;
                }
            }

            foreach (var entry in LoadChildrenBatched<T>(parentLink, defaultCulture))
            {
                yield return entry;
            }
        }

        internal IEnumerable<T> LoadChildrenBatched<T>(ContentReference parentLink, CultureInfo defaultCulture) where T : IContent
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