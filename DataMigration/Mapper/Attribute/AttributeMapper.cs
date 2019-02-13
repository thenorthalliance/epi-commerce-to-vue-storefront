using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.ContentProperty.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Mapper.Attribute
{
    public class AttributeMapper : IMapper
    {
        public Entity Map(CmsObjectBase cmsObject)
        {
            if (!(cmsObject is EpiContentProperty source))
            {
                return null;
            }

            return new Output.ElasticSearch.Entity.Attribute.Model.Attribute(source);
        }
    }
}   