using DataMigration.Input.Episerver.Category.Service;
using DataMigration.Input.Episerver.Common.Service;
using DataMigration.Input.Episerver.ContentProperty.Service;
using DataMigration.Input.Episerver.Product.Service;

namespace DataMigration.Input.Episerver
{
    public static class ContentServiceFactory
    {
        public static IContentService Create<T>() where T: class
        {
            var entityType = typeof(T);

            if(entityType == typeof(Output.ElasticSearch.Entity.Category.Model.Category))
                return new CategoryService();

            if (entityType == typeof(Output.ElasticSearch.Entity.Product.Model.Product))
                return new ProductService();

            if (entityType == typeof(DataMigration.Output.ElasticSearch.Entity.Attribute.Model.Attribute))
                return new PropertyService();

            return null;
        }
    }
}
