using System.Web;
using System.Web.Routing;

namespace SparkyTestHelpers.AspNetMvc.UnitTests.Routing
{
    public class LegacyUrlRoute : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (httpContext.Request.RawUrl.Contains(".aspx"))
            {
                HttpResponseBase response = httpContext.Response;
                response.Status = "302 Redirect";
                response.RedirectLocation = "Home/LegacyRedirect";
                response.End();
            }

            return null;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}
