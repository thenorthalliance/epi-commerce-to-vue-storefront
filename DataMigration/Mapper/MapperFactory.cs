using DataMigration.Output.ElasticSearch.Entity.Attribute.Model;
using DataMigration.Output.ElasticSearch.Entity.Category.Model;
using DataMigration.Output.ElasticSearch.Entity.Product.Model;

namespace DataMigration.Mapper
{
    public class MapperFactory
    {
        public static IMapper<T> Create<T>() where T: class
        {
            var docType = typeof(T);

            if (docType == typeof(Product))
                return (IMapper<T>) new ProductMapper();

            if (docType == typeof(Category))
                return (IMapper<T>)new CategoryMapper();

            if (docType == typeof(Attribute))
                return (IMapper<T>)new AttributeMapper();

            return null;
        }
    }
}