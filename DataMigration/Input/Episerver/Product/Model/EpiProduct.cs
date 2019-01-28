using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Input.Episerver.Product.Model
{
    public class EpiProduct : CmsObjectBase
    {
        public override EntityType EntityType => EntityType.Product;

    }
}