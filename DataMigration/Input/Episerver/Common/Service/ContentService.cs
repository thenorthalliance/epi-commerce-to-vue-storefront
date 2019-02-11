using System.Collections.Generic;
using System.Globalization;
using DataMigration.Input.Episerver.Common.Model;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Common.Service
{
    public abstract class ContentService
    {
        public abstract IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level = 2); 
    }
}