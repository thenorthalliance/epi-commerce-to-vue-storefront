using System.Collections.Generic;
using System.Security.Claims;

namespace EPiServer.VueStorefrontApiBridge.Authorization.Model
{
    public class TokenValidationResult
    {
        public IEnumerable<Claim> Claims { get; set; }
        public string AuthType { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
    }
}