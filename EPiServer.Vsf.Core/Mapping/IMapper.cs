namespace EPiServer.Vsf.Core.Mapping
{
    public interface IMapper<TSource, TDestination> where TDestination : class
    {
        TDestination Map(TSource source);
    }
}