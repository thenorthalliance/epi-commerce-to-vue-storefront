using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Mapper.Category
{
    public class CategoryMapper : IMapper
    {

        public Entity Map(CmsObjectBase cmsObject)
        {
            var source = cmsObject as EpiCategory;
           return new Output.ElasticSearch.Entity.Category.Model.Category()
           {
               Id = cmsObject.Id
           };
            
        }
    }
}