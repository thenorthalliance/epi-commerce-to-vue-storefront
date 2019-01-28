using DataMigration.Mapper.Attribute;
using DataMigration.Mapper.Category;
using DataMigration.Mapper.Product;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Mapper
{
    public class MapperFactory
    {
        public static IMapper Create(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Category:
                    return new CategoryMapper();
                case EntityType.Product:
                    return new ProductMapper();
                case EntityType.Attribute:
                    return new AttributeMapper();
                default:
                    return null;
            }
        }
    }
}