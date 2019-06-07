using BraintreeHttp;
using EPiServer.Vsf.Core.Payments;
using PayPalCheckoutSdk.Core;

namespace EPiServer.Vsf.Core.Services
{
    public class PayPalClient
    {
        /**
            Set up PayPal environment with sandbox credentials.
            In production, use ProductionEnvironment.
         */
        public static PayPalEnvironment Environment()
        {
            var configuration = new PayPalConfiguration();
            return new SandboxEnvironment(configuration.ClientId, configuration.Secret);
        }

        /**
            Returns PayPalHttpClient instance to invoke PayPal APIs.
         */
        public static HttpClient Client()
        {
            return new PayPalHttpClient(Environment());
        }

        public static HttpClient Client(string refreshToken)
        {
            return new PayPalHttpClient(Environment(), refreshToken);
        }
    }
}
