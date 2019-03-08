using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.Product.Model;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;

namespace DataMigration.Mapper
{
    public class ProductMapper : IMapper<Product>
    {
        public Product Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiProduct source))
            {
                return null;
            }

            return new Product(source);
        }
    }
}