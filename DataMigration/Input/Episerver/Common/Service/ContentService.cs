using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataMigration.Input.Episerver.Common.Model;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;

namespace DataMigration.Input.Episerver.Common.Service
{
    public abstract class ContentService
    {
        public abstract IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo); 

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
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var start = 0;
            while (true)
            {
                var batch = contentLoader.GetChildren<T>(parentLink, defaultCulture, start, 50);
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