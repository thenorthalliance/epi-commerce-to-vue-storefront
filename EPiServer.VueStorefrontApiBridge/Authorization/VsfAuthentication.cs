using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Mvc;
using EPiServer.Logging;

namespace EPiServer.VueStorefrontApiBridge.Authorization
{
    public class VsfAuthentication : IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {
                var tokenString = GetTokenString(context);
                if (string.IsNullOrEmpty(tokenString))
                    return Task.CompletedTask;
                
                var tokenProvider = (IUserTokenProvider)DependencyResolver.Current.GetService(typeof(IUserTokenProvider));

                if (tokenProvider.ValidateToken(tokenString, out var validationResult))
                    context.Principal = new ClaimsPrincipal(new ClaimsIdentity(validationResult.Claims, validationResult.AuthType));
                else
                    LogManager.GetLogger(GetType()).Debug($"Token validation failed: {validationResult.ErrorMessage}");
            }
            catch (Exception e)
            {
                LogManager.GetLogger(GetType()).Error("Vuestorefront authentication failed.", e);
            }
            
            return Task.CompletedTask;
        }

        private string GetTokenString(HttpAuthenticationContext context)
        {
            var parsedQuery = context.Request.RequestUri.ParseQueryString();
            return parsedQuery.Get("token");
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}