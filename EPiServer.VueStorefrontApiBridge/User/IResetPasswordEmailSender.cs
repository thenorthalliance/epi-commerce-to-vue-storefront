using System.Threading.Tasks;

namespace EPiServer.VueStorefrontApiBridge.User
{
    public interface IResetPasswordEmailSender
    {
        Task<bool> Send(string userEmail);
    }
}