using System;

namespace EPiServer.Vsf.Core.Mapping
{
    public interface IMapperSetup
    {
        bool TryGetMapperType(Type objectType, out Type mapperType);
    }
}