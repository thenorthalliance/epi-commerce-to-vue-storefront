using System.Threading.Tasks;

namespace EPiServer.Vsf.Core.Mapping
{
    public interface IMapper
    {
        Task<object> Map(object input);
    }
}