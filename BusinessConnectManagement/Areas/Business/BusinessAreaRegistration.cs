using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Business
{
    public class BusinessAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Business";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Business Login",
                "doanh-nghiep/dang-nhap",
                new { controller = "Auth", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Business.Controllers" }
            );

            context.MapRoute(
               "Business Logout",
               "doanh-nghiep/dang-xuat",
               new { controller = "Auth", action = "Logout", id = UrlParameter.Optional },
               namespaces: new[] { "BusinessConnectManagement.Areas.Business.Controllers" }
           );

            context.MapRoute(
                "Business Home",
                "doanh-nghiep/trang-chu",
                new { controller = "Business", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Business.Controllers" }
            );

            context.MapRoute(
                "Business_default",
                "Business/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Business.Controllers" }
            );
        }
    }
}