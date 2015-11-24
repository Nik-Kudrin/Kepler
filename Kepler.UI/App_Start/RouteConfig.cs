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
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Kepler", action = "Index", id = UrlParameter.Optional}
                );
            routes.MapRoute(
                name: "Build",
                url: "{controller}/{Project}/{Branch}/{BuildId}",
                defaults:
                    new
                    {
                        controller = "Kepler",
                        action = "PartialBuildView",
                        Project = UrlParameter.Optional,
                        Branch = UrlParameter.Optional,
                        BuildId = UrlParameter.Optional
                    }
                );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}