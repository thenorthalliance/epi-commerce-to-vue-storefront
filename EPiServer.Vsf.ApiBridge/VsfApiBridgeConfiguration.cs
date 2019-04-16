using System.Configuration;

namespace EPiServer.Vsf.ApiBridge
{
    public class VsfApiBridgeConfiguration  : ConfigurationSection
    {
        [ConfigurationProperty("auth.signingKey")]
        public string IssuerSigningKey => (string) this["auth.signingKey"];

        [ConfigurationProperty("auth.issuer")]
        public string ValidIssuer => (string)this["auth.issuer"];

        [ConfigurationProperty("auth.audience")]
        public string ValidAudience => (string)this["auth.audience"];

        [ConfigurationProperty("auth.accessTokenExpiration")]
        public int AccessTokenExpirationMinutes => (int)this["auth.accessTokenExpiration"];
    }
}