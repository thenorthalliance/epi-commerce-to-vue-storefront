using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;
using EPiServer.Commerce.Catalog.ContentTypes;

namespace DataMigration.Input.Episerver.Product.Model
{
    public class EpiProduct : CmsObjectBase
    {
        public override EntityType EntityType => EntityType.Product;
        public ProductContent ProductContent { get; set; }
        public new int Id => ProductContent.ContentLink.ID;
    }
}