using DataMigration.Input.Episerver.Category.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity.Category.Model;

namespace DataMigration.Mapper
{
    public class CategoryMapper : IMapper<Category>
    {
        public Category Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiCategory source))
            {
                return null;
            }

            return new Category(source);
        }
    }
}