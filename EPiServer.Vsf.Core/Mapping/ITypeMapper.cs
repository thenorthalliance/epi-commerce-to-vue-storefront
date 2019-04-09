using System;
using System.Threading.Tasks;

namespace EPiServer.Vsf.Core.Mapping
{
    public interface ITypeMapper
    {
        Type InputType { get; }
        Type OutputType { get; }

        Task<object> Map(object input);
    }
}