﻿using System.Web.Mvc;
using System.Web.Routing;

namespace Kepler.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "BuildOrBranch",
                url: "{buildOrBranch}",
                defaults: new {controller = "Kepler", action = "Index", buildOrBranch = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Admin",
                url: "admin/image-workers",
                defaults: new {controller = "Kepler", action = "Index", parameter = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Admin-Diff-Image",
                url: "admin/source-image-path",
                defaults: new {controller = "Kepler", action = "Index", parameter = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "Admin-Service-Url",
                url: "admin/service-url",
                defaults: new { controller = "Kepler", action = "Index", parameter = UrlParameter.Optional }
                );

			routes.MapRoute(
                name: "Admin-Scheduler",
                url: "admin/scheduler",
                defaults: new { controller = "Kepler", action = "Index", parameter = UrlParameter.Optional }
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