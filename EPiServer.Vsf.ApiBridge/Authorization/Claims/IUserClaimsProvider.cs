using System.Collections.Generic;
using System.Security.Claims;
using EPiServer.Vsf.Core.ApiBridge.Model.User;

namespace EPiServer.Vsf.ApiBridge.Authorization.Claims
{
    public interface IUserClaimsProvider<TUser> where TUser: VsfUser
    {
        IEnumerable<Claim> GetClaims(TUser user);
    }
}