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
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                actionContext.Response.Content = new ObjectContent(typeof(VsfErrorResponse), 
                        new VsfErrorResponse("You did not sign in correctly or your account is temporarily disabled."),
                        new JsonMediaTypeFormatter());
            }
        }
    }
}