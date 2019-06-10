using System.Net.Http.Headers;
using System.Threading.Tasks;
using BraintreeHttp;
using PayPalCheckoutSdk.Core;
using HttpClient = BraintreeHttp.HttpClient;

namespace EPiServer.Vsf.Core.Services
{
    public class PayPalHttpClient : HttpClient
    {
        public PayPalHttpClient(PayPalEnvironment environment)
          : base(environment)
        {
            AddInjector(new GzipInjector());
        }

        public override async Task<HttpResponse> Execute(HttpRequest request)
        {
            if (request.Headers.Contains("Authorization") || request is AccessTokenRequest || request is RefreshTokenRequest)
                return await base.Execute(request);

            var accessTokenResponse = await GetAccessTokenAsync();
            var accessToken = accessTokenResponse.Result<AccessToken>();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);

            return await base.Execute(request);
        }

        private async Task<HttpResponse> GetAccessTokenAsync()
        {
            return await Execute(new AccessTokenRequest((PayPalEnvironment)environment));
        }

        private class GzipInjector : IInjector
        {
            public void Inject(HttpRequest request)
            {
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            }
        }
    }
}
