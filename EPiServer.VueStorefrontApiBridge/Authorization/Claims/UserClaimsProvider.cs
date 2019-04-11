using System.Collections.Generic;
using System.Security.Claims;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization.Claims
{
    public class UserClaimsProvider<TUser> : IUserClaimsProvider<TUser> where TUser : UserModel
    {
        public IEnumerable<Claim> GetClaims(TUser user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Id)
            };
        }
    }
}