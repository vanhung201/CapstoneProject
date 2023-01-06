using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Mentor.Controllers
{
    public class MentorHomeController : Controller
    {
        // GET: Mentor/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}