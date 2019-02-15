using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.VueStorefrontApiBridge.ApiModel;

namespace EPiServer.VueStorefrontApiBridge.Mapper.User
{
    public interface IUserMapper
    {
        UserModel MapUser(ApplicationUser user);
    }
}