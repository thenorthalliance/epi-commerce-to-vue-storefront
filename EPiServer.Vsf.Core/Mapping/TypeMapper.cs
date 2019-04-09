using System;
using System.Threading.Tasks;

namespace EPiServer.Vsf.Core.Mapping
{
    public abstract class TypeMapper<TInput, TOutput> : ITypeMapper
    {
        public Type InputType
        {
            get => typeof(TInput);
        }

        public Type OutputType
        {
            get => typeof(TOutput);
        }

        public async Task<TOutput> Map(TInput input)
        {
            return (TOutput)await Map((object) input);
        }

        public abstract Task<object> Map(object input);
    }
}