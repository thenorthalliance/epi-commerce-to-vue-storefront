using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Input.Episerver.Category.Model
{
    public class EpiCategory : CmsObjectBase
    {
        public override EntityType EntityType => EntityType.Category;
    }
}