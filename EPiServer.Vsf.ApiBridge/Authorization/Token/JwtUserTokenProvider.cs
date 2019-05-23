using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPiServer.Vsf.ApiBridge.Authorization.Token.Model;
using EPiServer.Vsf.ApiBridge.Utils;

namespace EPiServer.Vsf.ApiBridge.Authorization.Token
{
    public class JwtUserTokenProvider : IUserTokenProvider
    {
        private readonly VsfApiBridgeConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public JwtUserTokenProvider(VsfApiBridgeConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }
        
        public Task<string> GenerateNewToken(IEnumerable<Claim> claims)
        {
            var signingCredentials = new SigningCredentials(_configuration.IssuerSigningKey.ToSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _configuration.ValidIssuer,
                audience: _configuration.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromSeconds(1)),
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