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
                name: "Default",
                url: "{controller}/{action}",
                defaults: new {controller = "Kepler", action = "Index"}
                );

            routes.MapRoute(
                name: "Build",
                url: "{project}/{branch}/{build}",
                defaults:
                    new
                    {
                        controller = "Kepler",
                        action = "Index",
                        project = "",
                        branch = "",
                        build = 1
                    }
                );
        }
    }
}