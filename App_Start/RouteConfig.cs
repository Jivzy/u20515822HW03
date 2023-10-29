using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace u20515822_HW03
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DeleteAuthor",
                url: "Maintain/DeleteAuthor/{id}",
                defaults: new { controller = "Maintain", action = "DeleteAuthor" }
            );
            routes.MapRoute(
                name: "DeleteAuthorConfirmed",
                url: "Maintain/DeleteAuthorConfirmed/{id}",
                defaults: new { controller = "Maintain", action = "DeleteAuthorConfirmed" }
);
        }
    }
}
