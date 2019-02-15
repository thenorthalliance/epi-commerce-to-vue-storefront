using System.Threading.Tasks;

namespace EPiServer.VueStorefrontApiBridge.Utils
{
    public interface IResetPasswordEmailSender
    {
        Task<bool> Send(string userEmail);
    }
}