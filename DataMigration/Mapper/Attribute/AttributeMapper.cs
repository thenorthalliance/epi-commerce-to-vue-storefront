using DataMigration.Input.Episerver.Attribute.Model;
using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Mapper.Attribute
{
    public class AttributeMapper : IMapper
    {
        public Entity Map(CmsObjectBase cmsObject)
        {
            var source = cmsObject as EpiAttribute;
            return new Output.ElasticSearch.Entity.Attribute.Model.Attribute()
            {
                Id = cmsObject.Id
            };
        }
    }
}