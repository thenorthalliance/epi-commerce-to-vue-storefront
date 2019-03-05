using System;
using System.Web;
using EPiServer.Core;
using EPiServer.Web.Routing;

namespace DataMigration.Input.Episerver.Common.Helpers
{
    public class UrlHelper
    {
        public static string GetUrl(ContentReference contentReference)
        {
            var urlString = UrlResolver.Current.GetUrl(contentReference);
            if (string.IsNullOrEmpty(urlString) || HttpContext.Current == null)
            {
                return urlString;
            }

            var uri = new Uri(urlString, UriKind.RelativeOrAbsolute);
            return uri.IsAbsoluteUri ? urlString : string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), uri);
        }
    }
}
