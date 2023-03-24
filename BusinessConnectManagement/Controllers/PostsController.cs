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
        public ActionResult Index(string SearchString = "")
        {
           
            if (SearchString != "")
            {
                var bu = db.Posts.Include(x => x.BusinessUser).Where(s => s.BusinessUser.BusinessName.ToUpper().Contains(SearchString.ToUpper()));
                var titles = db.Posts.Include(x => x.BusinessUser).Where(s => s.Title.ToUpper().Contains(SearchString.ToUpper()));
                if (titles.Any() == false)
                {
                    var tif = titles.Where(s => s.Form.ToUpper().Contains(SearchString.ToUpper()));
                    if (bu.Any() == false)
                    {
                        TempData["AlertMessage"] = "Không tìm thấy bài viết với từ khóa tìm kiếm " + "'" + SearchString + "'";
                    }
                    else
                    {
                        return View(bu.ToList());
                    }

                }
                return View(titles.ToList());

            }

            var posts = db.Posts.Include(p => p.BusinessUser).Include(p => p.Semester).Include(p => p.VanLangUser).Include(p => p.VanLangUser1);
            return View(posts.ToList());
        }
        public ActionResult Search(int? page,string SearchString = "", string Form = "", string Major = "")
        {
            
           
            ViewBag.Major = db.Majors.ToList();
            var bu = db.Posts.Include(x => x.BusinessUser).Where(s => s.BusinessUser.BusinessName.ToUpper().Contains(Form.ToUpper()));
            var titles = db.Posts.Include(x => x.BusinessUser).Where(s => s.Title.ToUpper().Contains(SearchString.ToUpper()));
            var form = db.Posts.Include(x => x.BusinessUser).Where(s => s.Form.ToUpper().Contains(Form.ToUpper()));
            var posts = db.Posts.Include(p => p.BusinessUser).Include(p => p.Semester).Include(p => p.VanLangUser).Include(p => p.VanLangUser1);
            int pageSize = 5;

            int pageNumber = (page ?? 1);
            if (Major == "")
            {
              
                if (Form == "")
                {
                  
                    if (SearchString != "")
                    {
                       
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
                        return View(form.ToList().ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        var tif = db.Posts.Include(x => x.BusinessUser).Where(s => s.Form.ToUpper().Contains(Form.ToUpper()) && s.Title.ToUpper().Contains(SearchString.ToUpper()));
                        return View(tif.ToList().ToPagedList(pageNumber, pageSize));
                    }
                }
            }
            else
            {
                if(Form != "")
                {
                    if(SearchString != "")
                    {
                        var getall = db.Posts.Include(x => x.BusinessUser).Where(s => s.Major.Major1.ToUpper().Contains(Major.ToUpper()) && s.Form.ToUpper().Contains(Form.ToUpper()) && s.Title.ToUpper().Contains(SearchString.ToUpper()));
                        return View(getall.ToList().ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        var getall = db.Posts.Include(x => x.BusinessUser).Where(s => s.Major.Major1.ToUpper().Contains(Major.ToUpper()) && s.Form.ToUpper().Contains(Form.ToUpper()));
                        return View(getall.ToList().ToPagedList(pageNumber, pageSize));
                    }
                }
                else
                {
                    if (SearchString != "")
                    {
                        var getall = db.Posts.Include(x => x.BusinessUser).Where(s => s.Major.Major1.ToUpper().Contains(Major.ToUpper()) && s.Title.ToUpper().Contains(SearchString.ToUpper()));
                        return View(getall.ToList().ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        var getall = db.Posts.Include(x => x.BusinessUser).Where(s => s.Major.Major1.ToUpper().Contains(Major.ToUpper()));
                        return View(getall.ToList().ToPagedList(pageNumber, pageSize));
                    }
                }
            }

           
           
        }
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            var email = User.Identity.Name;
          
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
            if (db.Registrations.Any(x => x.Post_ID == id && x.Email_VanLang == email))
            {
                ViewBag.registedID = db.Registrations.Where(x => x.Post_ID == id && x.Email_VanLang == email).First().ID;
                ViewBag.regStatus = db.Registrations.Where(x => x.Post_ID == id && x.Email_VanLang == email).First().StatusRegistration;
            }
            ViewBag.Post = db.Posts.Include(p => p.BusinessUser).Include(p => p.Semester).Include(p => p.VanLangUser).Include(p => p.VanLangUser1).OrderBy(x => Guid.NewGuid()).Take(5).ToList();
          
            var postPosition = db.PostInternshipTopics.Where(p => p.Post_ID == id).ToList();
          
            var position = db.InternshipTopics.ToList();

            List<PostPositionDetail> postPositionDetails= new List<PostPositionDetail>();
            foreach (var item in position)
            {
                bool isExist = postPosition.Where(x => x.InternshipTopic_ID == item.ID).Any();
                postPositionDetails.Add(new PostPositionDetail { Name = item.InternshipTopicName, Id = item.ID, status = isExist });;

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
