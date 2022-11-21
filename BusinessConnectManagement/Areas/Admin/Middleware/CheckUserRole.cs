using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Admin.Middleware
{
    public class CheckUserRole : Controller
    {
        public ActionResult RedirectToPage(int role)
        {
            if (role == 1)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (role == 2)
            {
                return RedirectToAction("Index", "Home", new { area = "Faculty" });
            }
            else if (role == 3)
            {
                return RedirectToAction("Index", "Home", new { area = "Mentor" });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "Student" });
            }
        }
    }
}