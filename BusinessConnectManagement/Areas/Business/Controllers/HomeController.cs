using BusinessConnectManagement.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Business.Controllers
{
    /*[BusinessVerification]*/
    public class HomeController : Controller
    {
        // GET: Business/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}