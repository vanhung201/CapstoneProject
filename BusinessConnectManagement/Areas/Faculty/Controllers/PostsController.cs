using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    public class PostsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Semester);
            return View(posts.ToList());
        }

        // GET: Faculty/Posts/Details/5
        public ActionResult Details(int? id)
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
            ViewBag.Business_ID = db.BusinessUsers.ToList();
            return View(post);
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
     
        public ActionResult Create(Post post, HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                using (var scope = new TransactionScope())
                {
                    post.PostImage = DateTime.Now.ToString("yymmssfff") + logo.FileName;
                    var path = Server.MapPath("~/Image/");
                    logo.SaveAs(path + post.PostImage);
                    post.PostDay = DateTime.Now;
                    post.ModifyDay = DateTime.Now;
                    var query = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
                    post.Email_ID = query.Email;
                    
                    db.Posts.Add(post);
                    db.SaveChanges();
                    scope.Complete();
                    
                }
                
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
       
        public ActionResult Edit( Post post, HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                if (logo != null)
                {
                    post.PostImage = DateTime.Now.ToString("yymmssfff") + logo.FileName;

                    var path = Server.MapPath("~/Image/");
                    logo.SaveAs(path + post.PostImage);
                    /*businessUser.Status = businessUser.Status_ID();*/
                }
                post.ModifyDay= DateTime.Now;

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
