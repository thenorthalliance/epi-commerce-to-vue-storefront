using System;
using System.Collections.Concurrent;

namespace EPiServer.Vsf.Core.Mapping
{
    public class MapperSetup : IMapperSetup
    {
        public readonly ConcurrentDictionary<Type, Type> TypeMapperMap = new ConcurrentDictionary<Type, Type>();

        public bool TryGetMapperType(Type objectType, out Type mapperType) =>
            TypeMapperMap.TryGetValue(objectType, out mapperType);
    }
}