using System.Collections.Generic;
using System.Threading.Tasks;
using EPiServer.Vsf.ApiBridge.Authorization.Model;

namespace EPiServer.Vsf.ApiBridge.Authorization.Token
{
    public class MemoryRefreshTokenRepository : IRefreshTokenRepository
    {
        private static readonly object Locker = new object();
        private static readonly Dictionary<string, RefreshToken> RefreshTokens = new Dictionary<string, RefreshToken>();

        public Task StoreToken(RefreshToken token)
        {
            lock (Locker)
            {
                RefreshTokens.Add(token.TokenId, token);
            }

            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetToken(string id)
        {
            lock (Locker)
            {
                return Task.FromResult(RefreshTokens[id]);
            }
        }

        public Task RemoveToken(string id)
        {
            lock (Locker)
            {
                RefreshTokens.Remove(id);
            }

            return Task.CompletedTask;
        }
    }
}