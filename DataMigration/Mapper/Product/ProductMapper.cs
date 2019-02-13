using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Mapper.Product
{
    public class ProductMapper : IMapper
    {
        public Entity Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiProduct source))
            {
                return null;
            }

            return new Output.ElasticSearch.Entity.Product.Model.Product(source);
        }
    }
}