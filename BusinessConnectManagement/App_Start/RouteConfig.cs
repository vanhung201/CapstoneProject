using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BusinessConnectManagement
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login Page",
                url: "quan-ly",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "Login for Manager",
                url: "quan-ly/dang-nhap",
                defaults: new { controller = "Account", action = "SignIn", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "Logout for Manager",
                url: "quan-ly/dang-xuat",
                defaults: new { controller = "Account", action = "SignOut", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "Student Homepage",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );
        }
    }
}
