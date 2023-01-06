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
                "Mentor_default",
                "Mentor/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BusinessConnectManagement.Areas.Mentor.Controllers" }
            );
        }
    }
}