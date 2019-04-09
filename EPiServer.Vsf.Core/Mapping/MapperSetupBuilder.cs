using System;

namespace EPiServer.Vsf.Core.Mapping
{
    public class MapperSetupBuilder
    {
        private readonly MapperSetup _setup;

        public class __MapperSetupBuilder_1 : MapperSetupBuilder
        {
            private readonly Type _mapperType;

            internal __MapperSetupBuilder_1(Type type, MapperSetup setup) : base(setup)
            {
                _mapperType = type;
            }

            public __MapperSetupBuilder_1 For<T>() where T : class
            {
                return For(typeof(T));
            }

            private __MapperSetupBuilder_1 For(Type type)
            {
                if (!_setup.TypeMapperMap.TryAdd(type, _mapperType))
                    throw new Exception($"Type '{type}' already mapped");
                
                return this;
            }
        }

        private MapperSetupBuilder(MapperSetup setup)
        {
            _setup = setup;
        }

        public static MapperSetupBuilder Create()
        {
            return new MapperSetupBuilder(new MapperSetup());
        }

        public __MapperSetupBuilder_1 Register<T>() where T : ITypeMapper
        {
            return new __MapperSetupBuilder_1(typeof(T), _setup);
        }

        private __MapperSetupBuilder_1 Register(Type type)
        {
            return new __MapperSetupBuilder_1(type, _setup);
        }

        public IMapperSetup Build()
        {
            return _setup;
        }
    }
}