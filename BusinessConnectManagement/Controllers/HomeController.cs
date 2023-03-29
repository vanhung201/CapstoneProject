using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace BusinessConnectManagement.Controllers
{
    public class HomeController : Controller
    {
        private BCMEntities db = new BCMEntities();
        // GET: Home
        public ActionResult Index( int? page)
        {
            ViewBag.MOUs = db.MOUs.ToList();
            ViewBag.Major = db.Majors.ToList();
            if (page == null) page= 1;

            var posts = (from post in db.Posts
                         select post).OrderByDescending(x => x.ID);


            int pageSize = 6;

            int pageNumber = (page ?? 1);

            ViewBag.Posts = posts;
            ViewBag.CountStudent = db.VanLangUsers.Count();
            ViewBag.CountPost = db.Posts.Count();
            ViewBag.CountBusiness = db.BusinessUsers.Count();
            ViewBag.MOU = db.MOUs.ToList();
            return View(posts.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Position()
        {
            return View();
        }
    }
}