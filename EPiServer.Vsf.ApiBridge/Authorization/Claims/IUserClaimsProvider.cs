using System.Collections.Generic;
using System.Security.Claims;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.Vsf.ApiBridge.Authorization.Claims
{
    public interface IUserClaimsProvider<TUser> where TUser: UserModel
    {
        IEnumerable<Claim> GetClaims(TUser user);
    }
}