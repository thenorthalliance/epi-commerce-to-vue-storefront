using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Input.Episerver.ContentProperty.Model;
using DataMigration.Output.ElasticSearch.Entity.Attribute.Model;

namespace DataMigration.Mapper
{
    public class AttributeMapper : IMapper<Attribute>
    {
        public Attribute Map(CmsObjectBase cmsObject)
        {
            return cmsObject is EpiContentProperty source 
                ? new Attribute(source) 
                : null;
        }
    }
}   