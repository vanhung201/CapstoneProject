using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Controllers
{
    public class PostsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.BusinessUser).Include(p => p.Semester).Include(p => p.VanLangUser).Include(p => p.VanLangUser1);
            return View(posts.ToList());
        }

        // GET: Posts/Details/5
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
            ViewBag.Post = db.Posts.Include(p => p.BusinessUser).Include(p => p.Semester).Include(p => p.VanLangUser).Include(p => p.VanLangUser1);
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
