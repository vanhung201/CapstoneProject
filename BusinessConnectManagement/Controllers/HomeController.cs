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
            if(page == null) page= 1;

            var posts = (from post in db.Posts
                         select post).OrderBy(x => x.ID);


            int pageSize = 5;

            int pageNumber = (page ?? 1);

            ViewBag.Posts = posts;
            ViewBag.CountStudent = db.VanLangUsers.Count();
            ViewBag.CountPost = db.Posts.Count();
            ViewBag.CountBusiness = db.BusinessUsers.Count();

            return View(posts.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Contact()
        {
            return View();
        }
    }
}