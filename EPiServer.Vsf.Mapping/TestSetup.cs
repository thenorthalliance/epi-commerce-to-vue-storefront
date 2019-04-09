using System;
using System.Threading.Tasks;
using EPiServer.Vsf.Core.Mapping;

namespace EPiServer.Vsf.Mapping
{
    public class TestMapper : TypeMapper<int, double>
    {
        public override Task<object> Map(object input)
        {
            throw new NotImplementedException();
        }
    }

   
}
