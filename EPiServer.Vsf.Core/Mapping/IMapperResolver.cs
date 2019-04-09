using System;

namespace EPiServer.Vsf.Core.Mapping
{
    public interface IMapperResolver
    {
        ITypeMapper Resolve(Type mapperType);
    }
}