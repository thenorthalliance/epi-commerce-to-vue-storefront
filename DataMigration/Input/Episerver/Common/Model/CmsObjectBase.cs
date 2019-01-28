using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Input.Episerver.Common.Model
{
    public abstract class CmsObjectBase
    {
        public abstract EntityType EntityType { get; }

        public int Id { get; set; }
    }
}