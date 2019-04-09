using System;
using EPiServer.ServiceLocation;
using EPiServer.Vsf.Core.Mapping;

namespace EPiServer.Vsf.Mapping
{
    public class ServiceLocatorMapperResolver : IMapperResolver
    {
        private readonly IServiceLocator _serviceLocator;


        public ServiceLocatorMapperResolver(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }
        public ITypeMapper Resolve(Type mapperType)
        {
            return (ITypeMapper)_serviceLocator.GetInstance(mapperType);
        }
    }
}