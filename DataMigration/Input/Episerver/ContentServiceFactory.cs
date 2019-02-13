using DataMigration.Input.Episerver.Category.Service;
using DataMigration.Input.Episerver.Common.Service;
using DataMigration.Input.Episerver.ContentProperty.Service;
using DataMigration.Input.Episerver.Product.Service;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Input.Episerver
{
    public static class ContentServiceFactory
    {
        public static IContentService Create(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Category:
                    return new CategoryService();
                case EntityType.Product:
                    return new ProductService();
                case EntityType.Attribute:
                    return new PropertyService();
                default:
                    return null;
            }
        }
    }
}
