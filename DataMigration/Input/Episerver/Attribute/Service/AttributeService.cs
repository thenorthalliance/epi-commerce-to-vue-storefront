using System.Collections.Generic;
using System.Globalization;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Attribute.Service
{
    public class AttributeService: ContentService
    {
        public override IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
