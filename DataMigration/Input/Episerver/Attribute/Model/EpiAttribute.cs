using System.Collections.Generic;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Input.Episerver.Attribute.Model
{
    public class EpiAttribute : CmsObjectBase
    {
        public override EntityType EntityType => EntityType.Attribute;
        public string Name { get; set; }
        public ICollection<string> Values { get; set; }
    }
}