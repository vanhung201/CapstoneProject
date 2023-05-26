using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin Home",
                "quan-ly/trang-chu",
                new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Authorize",
                "quan-ly/phan-quyen",
                new { controller = "AdminHome", action = "AuthorizeList", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Admin.Controllers" }
            );
            context.MapRoute(
                "Student List Mentor",
                "quan-ly/giao-vien/sinh-vien",
                new { controller = "InternshipResultsMentor", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Mentor.Controllers" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Admin.Controllers" }
            );
        }
    }
}