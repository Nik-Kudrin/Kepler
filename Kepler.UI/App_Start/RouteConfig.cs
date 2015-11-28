using System.Web.Mvc;
using System.Web.Routing;

namespace Kepler.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Build",
                url: "{buildOrBranch}",
                defaults: new {controller = "Kepler", action = "Index", buildOrBranch = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Admin",
                url: "admin/image-workers",
                defaults: new {controller = "Admin", action = "Index", parameter = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Admin-Diff-Image",
                url: "admin/diff-image",
                defaults: new {controller = "Admin", action = "Index", parameter = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{*parameter}",
                defaults: new {controller = "Kepler", action = "Index", parameter = UrlParameter.Optional}
                );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}