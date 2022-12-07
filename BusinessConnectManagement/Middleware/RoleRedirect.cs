using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Middleware
{
    public class RoleRedirect : Controller
    {
        public ActionResult Redirect(int role)
        {
            switch (role)
            {
                case 1:
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                case 2:
                    return RedirectToAction("Index", "Home", new { area = "Faculty" });
                case 3:
                    return RedirectToAction("Index", "Home", new { area = "Mentor" });
                default:
                    return RedirectToAction("Index", "Home", new { area = "Student" });
            }
        }
    }
}