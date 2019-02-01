using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace DataMigration.Helpers
{
    public class UrlHelper
    {
        private static readonly UrlResolver UrlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
        public static string GetUrl(ContentReference contentReference)
        {
            return UrlResolver.GetUrl(contentReference);
        }
    }
}
