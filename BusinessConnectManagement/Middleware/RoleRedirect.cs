using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Middleware
{
    public class RoleRedirect : Controller
    {
        public ActionResult AutoRedirect(string role)
        {
            switch (role)
            {
                case "Admin":
                    return RedirectToAction("Index", "AdminHome", new { area = "Admin" });
                case "Faculty":
                    return RedirectToAction("Index", "FacultyHome", new { area = "Faculty" });
                case "Mentor":
                    return RedirectToAction("Index", "MentorHome", new { area = "Mentor" });
                default:
                    return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
    }
}