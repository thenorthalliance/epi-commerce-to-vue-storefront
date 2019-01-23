using System.Collections.Generic;
using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization
{
    public class MemoryRefreshTokenRepo : IRefreshTokenRepo
    {
        private static readonly object _locker = new object();
        private static readonly Dictionary<string, RefreshToken> _refreshTokens = new Dictionary<string, RefreshToken>();

        public Task StoreToken(RefreshToken token)
        {
            lock (_locker)
            {
                _refreshTokens.Add(token.TokenId, token);
            }

            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetToken(string id)
        {
            lock (_locker)
            {
                return Task.FromResult(_refreshTokens[id]);
            }
        }

        public Task RemoveToken(string id)
        {
            lock (_locker)
            {
                _refreshTokens.Remove(id);
            }

            return Task.CompletedTask;
        }
    }
}