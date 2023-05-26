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

        public ActionResult ChangeYearStudy()
        {
            var curYearStudy = db.YearStudies.Where(x => x.Status == true).FirstOrDefault();
            var curSemester = db.Semesters.Where(x => x.Status == true).FirstOrDefault();
            var yearStudies = db.YearStudies.Where(x => x.Status == false && x.StartDate >= DateTime.Now).ToList();
            var message = "";

            if (curYearStudy.EndDate < DateTime.Now)
            {
                var nearestYear = yearStudies.OrderBy(x => Math.Abs((x.StartDate - DateTime.Now)?.TotalDays ?? 0)).FirstOrDefault();
                if (nearestYear != null)
                {
                    var nearestSemester = db.Semesters.Where(x => x.YearStudy_ID == nearestYear.ID).FirstOrDefault();
                    curYearStudy.Status = false;
                    db.Entry(curYearStudy).State = EntityState.Modified;
                    curSemester.Status = false;
                    db.Entry(curSemester).State = EntityState.Modified;
                    nearestYear.Status = true; // Set the nearest year's status to true
                    db.Entry(nearestYear).State = EntityState.Modified;
                    if(nearestSemester != null)
                    {
                        nearestSemester.Status = true;
                    } else
                    {
                        Semester semester = new Semester();
                        semester.YearStudy_ID = nearestYear.ID;
                        semester.Semester1 = "Semester";
                        semester.StartDate = DateTime.Now;
                        semester.EndDate = DateTime.Now.AddDays(105);
                        semester.Status = true;
                        db.Semesters.Add(semester);
                    }
                    db.SaveChanges();
                    message = "Nearest year found: " + nearestYear.YearStudy1;
                }
                else
                {
                    message = "No year studies found";
                }
            }
            else
            {
                message = "nothing change";
            }

            db.SaveChanges(); // Save the changes to the database

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeSemester()
        {
            var curYearStudy = db.YearStudies.Where(x => x.Status == true).FirstOrDefault();
            var curSemester = db.Semesters.Where(x => x.Status == true).FirstOrDefault();
            var semestersOfYear = db.Semesters.Where(x => x.YearStudy_ID == curYearStudy.ID && x.EndDate >= DateTime.Now).ToList();
            var message = "";
            if(curSemester.EndDate < DateTime.Now)
            {
                var nearestSemester = semestersOfYear.OrderBy(x => Math.Abs((x.StartDate - DateTime.Now)?.TotalDays ?? 0)).FirstOrDefault();
                if(nearestSemester != null)
                {
                    nearestSemester.Status = true;
                    db.Entry(nearestSemester).State = EntityState.Modified;

                    curSemester.Status = false;
                    db.Entry(curSemester).State = EntityState.Modified;
                    db.SaveChanges();
                    message = "update current semester";
                } else
                {
                    curSemester.Status = false;
                    db.Entry(curSemester).State = EntityState.Modified;
                    Semester semester = new Semester();
                    semester.YearStudy_ID = curYearStudy.ID;
                    semester.Semester1 = "Semester";
                    semester.StartDate = DateTime.Now;
                    semester.EndDate = DateTime.Now.AddDays(105);
                    semester.Status = true;
                    db.Semesters.Add(semester);
                    db.SaveChanges();
                    message = "add new semester for current year";
                }
            } else
            {
                message = "nothing change";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}