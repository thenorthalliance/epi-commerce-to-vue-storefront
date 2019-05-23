using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using EPiServer.Vsf.Core.ApiBridge.Model;

namespace EPiServer.Vsf.ApiBridge.Authorization
{
    public class VsfAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent(typeof(VsfCustomResponse<string>),
                        new VsfCustomResponse<string>(500, "not authorized"),
                        new JsonMediaTypeFormatter())
                };
            }
        }
    }
}