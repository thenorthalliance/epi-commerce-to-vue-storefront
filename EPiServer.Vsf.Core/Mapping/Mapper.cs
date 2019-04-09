using System;
using System.Threading.Tasks;

namespace EPiServer.Vsf.Core.Mapping
{
    public class Mapper : IMapper
    {
        private readonly IMapperSetup _setup;
        private readonly IMapperResolver _mapperResolver;

        public Mapper(IMapperSetup setup, IMapperResolver mapperResolver)
        {
            _setup = setup;
            _mapperResolver = mapperResolver;
        }

        public Task<object> Map(object input)
        {
            if (input == null)
                return Task.FromResult((object)null);

            if(!_setup.TryGetMapperType(input.GetType(), out var mapperType))
                throw new Exception($"Missing mapperType for type '{input.GetType()}'");

            var mapper = _mapperResolver.Resolve(mapperType);
            if(mapper == null)
                throw new Exception($"Missing mapper for type '{input.GetType()}'");

            return mapper.Map(input);
        }
    }
}