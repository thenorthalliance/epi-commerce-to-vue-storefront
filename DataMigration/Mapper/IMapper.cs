using DataMigration.Input.Episerver.Common.Model;
using DataMigration.Output.ElasticSearch.Entity;

namespace DataMigration.Mapper
{
    public interface IMapper
    {
        Entity Map(CmsObjectBase cmsObject);
    }
}