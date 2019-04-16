using System.Collections.Generic;
using System.Security.Claims;
using EPiServer.Vsf.Core.ApiBridge.Model;
using EPiServer.Vsf.Core.ApiBridge.Model.User;

namespace EPiServer.Vsf.ApiBridge.Authorization.Claims
{
    public class UserClaimsProvider<TUser> : IUserClaimsProvider<TUser> where TUser : VsfUser
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