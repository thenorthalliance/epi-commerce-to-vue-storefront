using System.Collections.Generic;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Input.Episerver.ContentProperty.Model
{
    public class EpiContentProperty : CmsObjectBase
    {
        public override EntityType EntityType => EntityType.Attribute;
        public string Name { get; set; }
        public ICollection<string> Values { get; set; }
    }
}