using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Admin.Controllers
{
    public class EmailController : Controller
    {
        // GET: Admin/Email
        public ActionResult EmailMentor()
        {
            return View();
        }

        public ActionResult EmailBusinessInterview()
        {
            return View();
        }
        public ActionResult EmailBusinessInternship()
        {
            return View();
        }
        public ActionResult EmailFacultyCV()
        {
            return View();
        }

        public ActionResult EmailFacultyMentor()
        {
            return View();
        }

        public ActionResult EmailUpdateAccount()
        {
            return View();
        }

    }
}