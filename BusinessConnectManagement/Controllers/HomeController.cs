using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;


namespace BusinessConnectManagement.Controllers
{
    public class HomeController : Controller
    {
        private BCMEntities db = new BCMEntities();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Post = db.Posts.Include(p => p.BusinessUser).Include(p => p.Semester).Include(p => p.VanLangUser).Include(p => p.VanLangUser1);
            ViewBag.CountStudent = db.VanLangUsers.Count();
            ViewBag.CountPost = db.Posts.Count();
            ViewBag.CountBusiness = db.BusinessUsers.Count();
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
    }
}