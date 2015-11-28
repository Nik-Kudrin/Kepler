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
                name: "Build",
                url: "{build}",
                defaults: new {controller = "Kepler", action = "Index", build = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "Branch",
                url: "{branch}",
                defaults: new {controller = "Kepler", action = "Index", branch = UrlParameter.Optional}
                );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{*id}",
                defaults: new {controller = "Kepler", action = "Index", id = UrlParameter.Optional}
                );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }
    }
}