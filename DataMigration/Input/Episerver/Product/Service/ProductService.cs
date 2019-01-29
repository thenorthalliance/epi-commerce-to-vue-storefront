using System;
using System.Collections.Generic;
using System.Globalization;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Common.Service;
using EPiServer.Core;

namespace DataMigration.Input.Episerver.Product.Service
{
    public class ProductService: ContentService
    {

        public override IEnumerable<CmsObjectBase> GetAll(ContentReference parentReference, CultureInfo cultureInfo, int level)
        {
            throw new NotImplementedException();
        }
    }
}
