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
        public ActionResult Index(int? page)
        {
          
            ViewBag.MOUs = db.MOUs.ToList();
            ViewBag.Major = db.Majors.ToList();
            if (page == null) page = 1;

            var posts = (from post in db.Posts
                         select post).OrderByDescending(x => x.ID);
           /* var totalQuantity = posts.Where(x => x.Quantity > 0).Sum(x => x.Quantity);*/

            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var filteredPosts = posts.Where(p => p.BusinessUser.Status_ID == 1)
                         .ToPagedList(pageNumber, pageSize);
            var filteredPostsHot = posts.Where(x => x.BusinessUser.Status_ID == 1).OrderByDescending(x => x.Registrations.Count).ToPagedList(pageNumber, pageSize);
            ViewBag.PostsHot = filteredPostsHot;
            ViewBag.Posts = filteredPosts;
            ViewBag.CountStudent = db.VanLangUsers.Count();
            ViewBag.CountPost = db.Posts.Count();
           
            ViewBag.CountBusiness = db.BusinessUsers.Count();
            ViewBag.MOU = db.MOUs.ToList();
            var getReID = db.Posts.FirstOrDefault();
            ViewBag.Registration = db.Registrations.Where(x => x.Post_ID == getReID.ID).Count();
            return View(filteredPosts);
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