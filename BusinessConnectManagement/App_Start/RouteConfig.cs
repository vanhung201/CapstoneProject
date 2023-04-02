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
                url: "trang-dang-nhap",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "Login for Manager",
                url: "dang-nhap",
                defaults: new { controller = "Account", action = "SignIn", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "Logout for Manager",
                url: "dang-xuat",
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
                name: "Student Information",
                url: "cap-nhat-thong-tin-tai-khoan",
                defaults: new { controller = "AccountOfStudent", action = "StudentInformation", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "User Post List",
                url: "tin-tuyen-dung",
                defaults: new { controller = "Posts", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "User Post Details",
                url: "chi-tiet-tin-tuyen-dung-{id}",
                defaults: new { controller = "Posts", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "Business Sign MOU",
                url: "doanh-nghiep-lien-ket",
                defaults: new { controller = "MOUs", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Controllers" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
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
