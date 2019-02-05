using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.VueStorefrontApiBridge.ApiModel;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public interface IUserMapper
    {
        UserModel MapUser(ApplicationUser user);
    }
}