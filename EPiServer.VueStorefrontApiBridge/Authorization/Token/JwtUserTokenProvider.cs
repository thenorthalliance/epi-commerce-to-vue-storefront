using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization.Token
{
    public class JwtUserTokenProvider : IUserTokenProvider
    {
        private readonly AuthTokenOptions _options;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public JwtUserTokenProvider(AuthTokenOptions options, IRefreshTokenRepository refreshTokenRepository)
        {
            _options = options;
            _refreshTokenRepository = refreshTokenRepository;
        }
        
        public Task<string> GenerateNewToken(IEnumerable<Claim> claims)
        {
            var signingCredentials = new SigningCredentials(_options.SecurityKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Task.FromResult(token);
        }

        public async Task<string> GenerateNewRefreshToken(IEnumerable<Claim> claims)
        {
            var nameId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if(nameId == null)
                throw new Exception("NameIdentifier claim is missing.");

            var tokenId = Guid.NewGuid().ToString();
            await _refreshTokenRepository.StoreToken(new RefreshToken
            {
                UserId = nameId,
                TokenId = tokenId
            });

            return tokenId;
        }

        public async Task<RefreshToken> GetRefreshToken(string refreshTokenId)
        {
            return await _refreshTokenRepository.GetToken(refreshTokenId);
        }
    }
}