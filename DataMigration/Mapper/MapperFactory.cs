using DataMigration.Mapper.Attribute;
using DataMigration.Mapper.Category;
using DataMigration.Mapper.Product;

namespace DataMigration.Mapper
{
    public class MapperFactory
    {
        public static IMapper<T> Create<T>() where T: class
        {
            var docType = typeof(T);

            if (docType == typeof(Output.ElasticSearch.Entity.Product.Model.Product))
                return (IMapper<T>) new ProductMapper();

            if (docType == typeof(Output.ElasticSearch.Entity.Category.Model.Category))
                return (IMapper<T>)new CategoryMapper();

            if (docType == typeof(Output.ElasticSearch.Entity.Attribute.Model.Attribute))
                return (IMapper<T>)new AttributeMapper();

            return null;
        }
    }
}