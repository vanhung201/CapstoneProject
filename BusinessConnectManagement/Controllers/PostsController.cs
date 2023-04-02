using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;
using PagedList;

namespace BusinessConnectManagement.Controllers
{
    public class PostsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        public class PostPositionDetail
        {
            public string Name { get; set; }
            public int Id { get; set; }

            public bool status { get; set; }
            public int IdI { get; set; }


        }

        // GET: Posts
        public ActionResult Index(int? page)
        {
            ViewBag.MOUs = db.MOUs.ToList();
            ViewBag.Major = db.Majors.ToList();
            if (page == null) page = 1;

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
        public ActionResult Search(int? page, string SearchString = "", string Form = "", string Major = "")
        {

            ViewBag.SearchString = SearchString;
            ViewBag.Form = Form;
            ViewBag.Major = Major;
            ViewBag.Major = db.Majors.ToList();
            var bu = db.Posts.Include(x => x.BusinessUser).Where(s => s.BusinessUser.BusinessName.ToUpper().Contains(Form.ToUpper()));
            var titles = db.Posts.Include(x => x.BusinessUser).Where(s => s.Title.ToUpper().Contains(SearchString.ToUpper()));
            var form = db.Posts.Include(x => x.BusinessUser).Where(s => s.Form.ToUpper().Contains(Form.ToUpper()));
            var posts = db.Posts.Include(p => p.BusinessUser).Include(p => p.Semester).Include(p => p.VanLangUser).Include(p => p.VanLangUser1);
            int pageSize = 6;

            int pageNumber = (page ?? 1);
            if (Major == "")
            {

                if (Form == "")
                {

                    if (SearchString != "")
                    {
                        if (!titles.Any())
                        {
                            TempData["Message"] = "Không có kết quả tìm kiếm với từ khóa: " + SearchString;
                            return RedirectToAction("Search", "Posts");
                        }
                        return View(titles.ToList().ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(posts.ToList().ToPagedList(pageNumber, pageSize));
                    }
                }
                else
                {
                    if (SearchString == "")
                    {
                        if (!form.Any())
                        {
                            TempData["Message"] = "Không có kết quả tìm kiếm với từ khóa: " + form;
                            return RedirectToAction("Search", "Posts");
                        }
                        return View(form.ToList().ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        var tif = db.Posts.Include(x => x.BusinessUser).Where(s => s.Form.ToUpper().Contains(Form.ToUpper()) && s.Title.ToUpper().Contains(SearchString.ToUpper()));
                        if (!tif.Any())
                        {
                            TempData["Message"] = "Không có kết quả tìm kiếm với từ khóa: " + SearchString + ", " + Form;
                            return RedirectToAction("Search", "Posts");
                        }
                        return View(tif.ToList().ToPagedList(pageNumber, pageSize));
                    }
                }
            }
            else
            {
                if (Form != "")
                {
                    if (SearchString != "")
                    {
                        var getall = db.Posts.Include(x => x.BusinessUser).Where(s => s.Major.Major1.ToUpper().Contains(Major.ToUpper()) && s.Form.ToUpper().Contains(Form.ToUpper()) && s.Title.ToUpper().Contains(SearchString.ToUpper()));
                        if (!getall.Any())
                        {
                            TempData["Message"] = "Không có kết quả tìm kiếm với từ khóa: " + Form + ", " + Major + ", " + SearchString;
                            return RedirectToAction("Search", "Posts");
                        }
                        return View(getall.ToList().ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        var getall = db.Posts.Include(x => x.BusinessUser).Where(s => s.Major.Major1.ToUpper().Contains(Major.ToUpper()) && s.Form.ToUpper().Contains(Form.ToUpper()));
                        if (!getall.Any())
                        {
                            TempData["Message"] = "Không có kết quả tìm kiếm với từ khóa: " + Form + ", " + Major;
                            return RedirectToAction("Search", "Posts");
                        }
                        return View(getall.ToList().ToPagedList(pageNumber, pageSize));
                    }
                }
                else
                {
                    if (SearchString != "")
                    {
                        var getall = db.Posts.Include(x => x.BusinessUser).Where(s => s.Major.Major1.ToUpper().Contains(Major.ToUpper()) && s.Title.ToUpper().Contains(SearchString.ToUpper()));
                        if (!getall.Any())
                        {
                            TempData["Message"] = "Không có kết quả tìm kiếm với từ khóa: " + SearchString + ", " + Major;
                            return RedirectToAction("Search", "Posts");
                        }
                        return View(getall.ToList().ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        var getall = db.Posts.Include(x => x.BusinessUser).Where(s => s.Major.Major1.ToUpper().Contains(Major.ToUpper()));
                        if (!getall.Any())
                        {
                            TempData["Message"] = "Không có kết quả tìm kiếm với từ khóa: " + Major;

                            return RedirectToAction("Search", "Posts");
                        }
                        return View(getall.ToList().ToPagedList(pageNumber, pageSize));
                    }
                }
            }



        }
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            var email = User.Identity.Name;
            ViewBag.isStudent = db.VanLangUsers.Any(x => x.Email == email && x.Role == "Student");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.isRegisted = db.Registrations.Any(x => x.Email_VanLang == email && x.Post_ID == id);
            ViewBag.isExist = db.Registrations.Any(x => x.Email_VanLang == email && x.InterviewResult == "Đang Thực Tập");
            if (db.Registrations.Any(x => x.Post_ID == id && x.Email_VanLang == email))
            {
                ViewBag.registedID = db.Registrations.Where(x => x.Post_ID == id && x.Email_VanLang == email).First().ID;
                ViewBag.regStatus = db.Registrations.Where(x => x.Post_ID == id && x.Email_VanLang == email).First().StatusRegistration;
            }
            ViewBag.Post = db.Posts.Include(p => p.BusinessUser).Include(p => p.Semester).Include(p => p.VanLangUser).Include(p => p.VanLangUser1).OrderBy(x => Guid.NewGuid()).Take(6).ToList();

            var postPosition = db.PostInternshipTopics.Where(p => p.Post_ID == id).ToList();

            var position = db.InternshipTopics.ToList();

            List<PostPositionDetail> postPositionDetails = new List<PostPositionDetail>();
            foreach (var item in position)
            {
                bool isExist = postPosition.Where(x => x.InternshipTopic_ID == item.ID).Any();
                postPositionDetails.Add(new PostPositionDetail { Name = item.InternshipTopicName, Id = item.ID, status = isExist }); ;

            }
            ViewBag.Position = postPositionDetails;

            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Email_ID = new SelectList(db.VanLangUsers, "Email", "FullName");
            ViewBag.Email_ID = new SelectList(db.VanLangUsers, "Email", "FullName");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Email_ID,Description,PostDay,ModifyDay,Semester_ID,PostImage,Business_ID")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", post.Business_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", post.Semester_ID);
            ViewBag.Email_ID = new SelectList(db.VanLangUsers, "Email", "FullName", post.Email_ID);
            ViewBag.Email_ID = new SelectList(db.VanLangUsers, "Email", "FullName", post.Email_ID);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", post.Business_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", post.Semester_ID);
            ViewBag.Email_ID = new SelectList(db.VanLangUsers, "Email", "FullName", post.Email_ID);
            ViewBag.Email_ID = new SelectList(db.VanLangUsers, "Email", "FullName", post.Email_ID);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Email_ID,Description,PostDay,ModifyDay,Semester_ID,PostImage,Business_ID")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", post.Business_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", post.Semester_ID);
            ViewBag.Email_ID = new SelectList(db.VanLangUsers, "Email", "FullName", post.Email_ID);
            ViewBag.Email_ID = new SelectList(db.VanLangUsers, "Email", "FullName", post.Email_ID);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
