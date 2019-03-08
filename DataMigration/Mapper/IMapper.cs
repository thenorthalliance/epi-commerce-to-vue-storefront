using DataMigration.Input.Episerver.Common.Model;

namespace DataMigration.Mapper
{
    public interface IMapper<T> where T:class
    {
        T Map(CmsObjectBase cmsObject);
    }
}