namespace DataMigration.Mapper
{
    public interface IMapper<TSource, TDestination> where TDestination : class
    {
        TDestination Map(TSource source);
    }
}