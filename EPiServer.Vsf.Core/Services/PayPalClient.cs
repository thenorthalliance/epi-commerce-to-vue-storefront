using EPiServer.Vsf.Core.Payments;
using PayPalCheckoutSdk.Core;

namespace EPiServer.Vsf.Core.Services
{
    public class PayPalClient
    {
        private static PayPalHttpClient _instance;
        private static readonly object _lock = new object();

        public static PayPalHttpClient Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var configuration = new PayPalConfiguration();
                        var environment = new SandboxEnvironment(configuration.ClientId, configuration.Secret);
                        _instance = new PayPalHttpClient(environment);
                    }
                    return _instance;
                }
            }

        }
    }
}
