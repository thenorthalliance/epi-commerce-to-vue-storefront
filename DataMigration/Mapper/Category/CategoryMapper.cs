using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Mapper.Category
{
    public class CategoryMapper : IMapper
    {
        public Entity Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiCategory source))
            {
                return null;
            }

            return new Output.ElasticSearch.Entity.Category.Model.Category(source);
        }
    }
}