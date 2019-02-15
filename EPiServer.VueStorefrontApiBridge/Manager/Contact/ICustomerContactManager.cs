using EPiServer.VueStorefrontApiBridge.ApiModel;

namespace EPiServer.VueStorefrontApiBridge.Manager.Contact
{
    public interface ICustomerContactManager
    {
        bool UpdateCustomerContact(string userId, UserModel updatedUser);
    }
}
