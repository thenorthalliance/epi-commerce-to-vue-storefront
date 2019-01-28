using System.IdentityModel.Tokens;

namespace EPiServer.VueStorefrontApiBridge.Authorization.Model
{
    public class AuthTokenOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public string AuthenticationType { get; set; } = "VueStorefronToken";

        public SymmetricSecurityKey SecurityKey { get; set; }
    }
}