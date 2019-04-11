using System.Collections.Generic;
using System.Security.Claims;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization.Claims
{
    public interface IUserClaimsProvider<TUser> where TUser: UserModel
    {
        IEnumerable<Claim> GetClaims(TUser user);
    }
}