using System;
using System.Web;
using EPiServer.Core;
using EPiServer.Web.Routing;

namespace EPiServer.Vsf.DataExport.Utils
{
    public static class UrlHelper
    {
        public static string GetUrl(this ContentReference contentReference)
        {
            if (contentReference == null)
                return string.Empty;

            var urlString = UrlResolver.Current.GetUrl(contentReference);
            
            if (string.IsNullOrEmpty(urlString) || HttpContext.Current == null)
                return urlString;

            var uri = new Uri(urlString, UriKind.RelativeOrAbsolute);
            return uri.IsAbsoluteUri ? urlString : string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), uri);
        }

        public static string GetAsThumbnailUrl(string url)
        {
            return string.IsNullOrEmpty(url) ? string.Empty : $"{url}/Thumbnail";
        }
    }
}
