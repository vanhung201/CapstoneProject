using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    [LoginVerification]
    public class PostsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/Posts
        [HttpGet]
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Semester);
            ViewBag.Business_ID = db.BusinessUsers.ToList();
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
                                id= post.ID,
                                title = post.Title,
                                email = post.VanLangUser.Email,
                                name = post.VanLangUser.FullName,
                                description = post.Description,
                                postday = post.PostDay,
                                modifyday=post.ModifyDay,
                                semeter = post.Semester.Semester1,
                                business = post.BusinessUser.BusinessName

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
                                business_id=post.Business_ID,
                                form = post.Form,
                                duedate = post.DueDate,
                                quatity = post.Quantity

                            });
            return Json(listData, JsonRequestBehavior.AllowGet);
          
           
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

        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    post.PostDay = (DateTime.Now).ToString();
                    post.ModifyDay = (DateTime.Now).ToString();
                    var query = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                    post.Email_ID = query.Email;

                    db.Posts.Add(post);
                    db.SaveChanges();
                    scope.Complete();

                }
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

        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.ModifyDay = (DateTime.Now).ToString(); 

                db.Entry(post).State = EntityState.Modified;
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
