using System.IdentityModel.Tokens;
using System.Text;

namespace EPiServer.Vsf.ApiBridge.Utils
{
    public static class SecurityExtensions
    {
        public static SymmetricSecurityKey ToSymmetricSecurityKey(this string key)
        {
            return new InMemorySymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}