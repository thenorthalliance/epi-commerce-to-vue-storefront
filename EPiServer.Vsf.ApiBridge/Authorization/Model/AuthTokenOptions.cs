using System.IdentityModel.Tokens;

namespace EPiServer.Vsf.ApiBridge.Authorization.Model
{
    public class AuthTokenOptions2
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public int AccessTokenExpirationMinutes { get; set; } = 60;

        public SymmetricSecurityKey SecurityKey { get; set; }
        
    }
}