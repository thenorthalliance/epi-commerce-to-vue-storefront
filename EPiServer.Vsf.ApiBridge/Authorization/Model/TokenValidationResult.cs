using System.Collections.Generic;
using System.Security.Claims;

namespace EPiServer.Vsf.ApiBridge.Authorization.Model
{
    public class TokenValidationResult
    {
        public IEnumerable<Claim> Claims { get; set; }
        public string AuthType { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
    }
}