using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Mapper.Product
{
    public class ProductMapper : IMapper
    {
        public Entity Map(CmsObjectBase cmsObject)
        {
            var source = cmsObject as EpiProduct;
            return new Output.ElasticSearch.Entity.Product.Model.Product()
            {
                Id = cmsObject.Id
            };
        }
    }
}