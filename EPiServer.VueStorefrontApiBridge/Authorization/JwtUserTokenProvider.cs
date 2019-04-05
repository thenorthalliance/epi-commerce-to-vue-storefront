using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPiServer.VueStorefrontApiBridge.ApiModel;
using EPiServer.VueStorefrontApiBridge.Authorization.Model;

namespace EPiServer.VueStorefrontApiBridge.Authorization
{
    public class JwtUserTokenProvider : IUserTokenProvider
    {
        private readonly AuthTokenOptions _options;
        private readonly IRefreshTokenRepo _refreshTokenRepo;

        public JwtUserTokenProvider(AuthTokenOptions options, IRefreshTokenRepo refreshTokenRepo)
        {
            _options = options;
            _refreshTokenRepo = refreshTokenRepo;
        }

        public Task<string> GenerateNewToken(UserModel user)
        {
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Id)
            }, _options.AuthenticationType);

            return Task.FromResult(GenerateTokenForClaims(claimsIdentity));
        }

        public async Task<string> GenerateNewRefreshToken(UserModel user)
        {
            var tokenId = Guid.NewGuid().ToString();
            await _refreshTokenRepo.StoreToken(new RefreshToken
            {
                UserId = user.Id,
                TokenId = tokenId
            });

            return tokenId;
        }

        public async Task<RefreshToken> GetRefreshToken(string refreshTokenId)
        {
            return await _refreshTokenRepo.GetToken(refreshTokenId);
        }

        public bool ValidateToken(string token, out TokenValidationResult validationResult)
        {
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = _options.SecurityKey,
                ValidAudience = _options.Audience,
                ValidIssuer = _options.Issuer
            };

            try
            {
                new JwtSecurityTokenHandler().ValidateToken(token, validationParameters,
                    out var validatedToken);

                validationResult = new TokenValidationResult
                {
                    AuthType = _options.AuthenticationType,
                    Claims = MapInboundClaims(((JwtSecurityToken) validatedToken).Claims)
                };

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

                validationResult = new TokenValidationResult
                {
                    AuthType = string.Empty,
                    Claims = new List<Claim>(),
                    HasError = true,
                    ErrorMessage = e.Message
                };

                return false;
            }
        }

        private string GenerateTokenForClaims(ClaimsIdentity claimsIdentity)
        {
            var signingCredentials = new SigningCredentials(_options.SecurityKey,
                SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                AppliesToAddress = _options.Audience,
                TokenIssuerName = _options.Issuer,
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                TokenType = "bearer"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(securityTokenDescriptor));
        }

        private static IEnumerable<Claim> MapInboundClaims(IEnumerable<Claim> claims)
        {
            return claims.Select(c =>
                JwtSecurityTokenHandler.InboundClaimTypeMap.ContainsKey(c.Type)
                    ? new Claim(JwtSecurityTokenHandler.InboundClaimTypeMap[c.Type], c.Value, c.ValueType, c.Issuer)
                    : c);
        }
    }
}