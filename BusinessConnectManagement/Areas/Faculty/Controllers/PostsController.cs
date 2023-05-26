using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using Microsoft.Ajax.Utilities;
using static BusinessConnectManagement.Areas.Faculty.Controllers.BusinessCooperationCategoriesController;
using static BusinessConnectManagement.Areas.Faculty.Controllers.BusinessUsersController;
using static BusinessConnectManagement.Controllers.PostsController;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    [LoginVerification]
    public class PostsController : Controller
    {
        private BCMEntities db = new BCMEntities();
        
        public class PostInternGet
        {
            public string name { get; set; }
            public int id { get; set; }
            public bool status { get; set; }
            public int quantity { get;set; }
            public string shortName { get;set; }
        }
        // GET: Faculty/Posts
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Business_ID = db.BusinessUsers.Where( x=> x.Status_ID == 1).ToList();
            ViewBag.Position = db.InternshipTopics.ToList();
            ViewBag.Major = db.Majors.ToList();
            var posts = db.Posts.ToList();
            return View(posts.ToList());
        }

        public JsonResult getDataList()
        {
            var listData = (from post in db.Posts
                            join e in db.VanLangUsers on post.Email_ID equals e.Email into email
                            join se in db.Semesters on post.Semester_ID equals se.ID into semeter
                            join bu in db.BusinessUsers on post.Business_ID equals bu.ID into business
                            select new
                            {
                                id = post.ID,
                                title = post.Title,
                                email = post.VanLangUser.Email,
                                name = post.VanLangUser.FullName,
                                description = post.Description,
                                postday = post.PostDay,
                                modifyday = post.ModifyDay,
                                semeter = post.Semester.Semester1,
                                business = post.BusinessUser.BusinessName,
                                duedate = post.DueDate,
                             
                            });
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
        // GET: Faculty/Posts/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.Business_ID = db.BusinessUsers.ToList();
            Post postt = db.Posts.Find(id);
            var listData = (from post in db.Posts
                            join e in db.VanLangUsers on post.Email_ID equals e.Email into email
                            join se in db.Semesters on post.Semester_ID equals se.ID into semeter
                            join bu in db.BusinessUsers on post.Business_ID equals bu.ID into business
                            where post.ID == id
                            select new
                            {
                                id = post.ID,
                                title = post.Title,
                                email = post.VanLangUser.Email,
                                name = post.VanLangUser.FullName,
                                description = post.Description,
                                postday = post.PostDay,
                                modifyday = post.ModifyDay,
                                semeter = post.Semester.Semester1,
                                business = post.BusinessUser.BusinessName,
                                business_id = post.Business_ID,
                                form = post.Form,
                                duedate = post.DueDate,
                                quatity = post.Quantity,
                                majorr = post.Major.Major1,
                                semes_id = post.Semester_ID,
                            });
            List<PostInternGet> postInternGets = new List<PostInternGet>();
            var PostPosition = db.InternshipTopics.ToList();
            var PostPositionList = db.PostInternshipTopics.Where(x => x.Post_ID == id).ToList();
            foreach (var item in PostPosition)
            {
                bool isExist = PostPositionList.Where(x => x.InternshipTopic_ID == item.ID).Any();
                int quantity = PostPositionList.FirstOrDefault(x => x.InternshipTopic_ID == item.ID)?.Quantity ?? 0;
                postInternGets.Add(new PostInternGet { name = item.InternshipTopicName, id = item.ID, status = isExist, quantity = quantity,shortName = item.ShortName });
            }
            ViewBag.PostIntern = postInternGets;
            return Json(new { listData, postIntern = ViewBag.PostIntern}, JsonRequestBehavior.AllowGet);
        }


        // GET: Faculty/Posts/Create
        public ActionResult Create()
        {
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Business_ID = db.BusinessUsers.ToList();
            return View();
        }

        // POST: Faculty/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]

        public ActionResult Create(Post post, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var checkSem = db.Semesters.Where(x => x.Status == true).FirstOrDefault();
                post.PostDay = string.Format("{0:dd/MM/yyyy hh:mm:ss}", DateTime.Now);
                post.ModifyDay = string.Format("{0:dd/MM/yyyy hh:mm:ss}", DateTime.Now);
                var query = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                post.Email_ID = query.Email;
                post.Semester_ID = checkSem.ID;
                db.Posts.Add(post);
                db.SaveChanges();
                string[] positions = form.GetValues("positions");
                for (int i = 0; i < positions.Length; i++)
                {
                    var pp = new PostInternshipTopic();
                    pp.Business_ID = post.Business_ID;
                    pp.Post_ID = post.ID;
                    pp.InternshipTopic_ID = Int32.Parse(positions[i]);

                    // Get the quantity for this position
                    int quantity = Int32.Parse(form["quantity-" + positions[i]]);
                    pp.Quantity = quantity;
                    db.PostInternshipTopics.Add(pp);
                }
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Thêm dữ liệu thành công</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");
            }

            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", post.Semester_ID);
            return View(post);
        }

    // GET: Faculty/Posts/Edit/5
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

            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", post.Semester_ID);
            return View(post);
        }

        // POST: Faculty/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateInput(false)]

        public ActionResult Edit(Post post, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var registration = db.Registrations.Where(x => x.Post_ID == post.ID).FirstOrDefault();
                var removePostIntern = db.PostInternshipTopics.Where(x => x.Post_ID == post.ID).ToList();
               
                foreach (var item in removePostIntern)
                {
                 db.PostInternshipTopics.Remove(item);
                }
                post.ModifyDay = (DateTime.Now).ToString();
                
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                string[] positions = form.GetValues("position");
                if (positions != null)
                {
                    for (int i = 0; i < positions.Length; i++)
                    {

                        var pp = new PostInternshipTopic();
                        pp.Business_ID = post.Business_ID;
                        pp.Post_ID = post.ID;
                        pp.InternshipTopic_ID = Int32.Parse(positions[i]);
                        int quantity = Int32.Parse(form["quantity-" + positions[i]]);
                        pp.Quantity = quantity;
                        db.PostInternshipTopics.Add(pp);

                    }
                }
               
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật thành công</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                return RedirectToAction("Index", "Posts");
            }
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", post.Semester_ID);
            return View(post);
        }

        // GET: Faculty/Posts/Delete/5
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

        // POST: Faculty/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (db.Registrations.Where(x => x.Post_ID == id).Any() == true)
            {
                TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Xóa không thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                return RedirectToAction("Index");
            }
            else
            {
                Post post = db.Posts.Find(id);
                db.Posts.Remove(post);
                var postInterns = db.PostInternshipTopics.Where(x => x.Post_ID == id).ToList();
                foreach (var postIntern in postInterns)
                {
                    db.PostInternshipTopics.Remove(postIntern);
                }
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Xóa thành công</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");
            }

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