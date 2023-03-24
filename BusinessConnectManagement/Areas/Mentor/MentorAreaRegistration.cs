using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Mentor
{
    public class MentorAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Mentor";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                 "Mentor Home",
                 "quan-ly/giao-vien/trang-chu",
                 new { controller = "MentorHome", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "BusinessConnectManagement.Areas.Mentor.Controllers" }
             );
            context.MapRoute(
                 "Student List",
                 "quan-ly/giao-vien/sinh-vien",
                 new { controller = "InternshipResultsMentor", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "BusinessConnectManagement.Areas.Mentor.Controllers" }
             );
            context.MapRoute(
               "Mentor_default",
               "Mentor/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "BusinessConnectManagement.Areas.Mentor.Controllers" }
           );
        }
    }
}