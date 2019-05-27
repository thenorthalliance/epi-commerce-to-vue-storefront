using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using EPiServer.Vsf.ApiBridge.Authorization.Token.Model;
using EPiServer.Vsf.DataAccess;

namespace EPiServer.Vsf.ApiBridge.Authorization.Token
{
    public class DatabaseRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly QuicksilverDbContext _dbContext;
        static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        public DatabaseRefreshTokenRepository(QuicksilverDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task StoreToken(RefreshToken token)
        {
            await SemaphoreSlim.WaitAsync(); //semaphore or should we use concurrency exception from EF
            try
            {
                var user = await _dbContext.AspNetUsers
                    .Include(x => x.RefreshToken)
                    .FirstOrDefaultAsync(x => x.Id == token.UserId);

                if (user != null)
                {
                    if (user.RefreshToken == null)
                    {
                        _dbContext.RefreshTokens.Add(new DataAccess.Model.RefreshToken
                        {
                            User = user,
                            Value = token.TokenId
                        });
                    }
                    else
                    {
                        user.RefreshToken.Value = token.TokenId;
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        public async Task<RefreshToken> GetToken(string id)
        {
            await SemaphoreSlim.WaitAsync(); //semaphore or should we use concurrency exception from EF
            try
            {
                var dbToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Value == id);
                if (dbToken != null)
                {
                    var refreshToken = new RefreshToken
                    {
                        TokenId = dbToken.Value,
                        UserId = dbToken.UserId
                    };

                    return refreshToken;
                }

                return null;

            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }

        public async Task RemoveToken(string id)
        {
            await SemaphoreSlim.WaitAsync(); //semaphore or should we use concurrency exception from EF
            try
            {
                var dbToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Value == id);
                if (dbToken != null)
                {
                    _dbContext.RefreshTokens.Remove(dbToken);
                }

                await _dbContext.SaveChangesAsync();
            }
            finally
            {
                SemaphoreSlim.Release();
            }
        }
    }
}