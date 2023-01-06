using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Business.Controllers
{
    [BusinessVerification]
    public class InternViewsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Business/InternViews
        public ActionResult Index()
        {
            var registrations = db.Registrations.Include(r => r.BusinessUser).Include(r => r.Post).Include(r => r.Semester).Include(r => r.VanLangUser);
           
            return View(registrations.ToList());
        }

        // GET: Business/InternViews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Registration registration = db.Registrations.Find(id);

            if (registration == null)
            {
                return HttpNotFound();
            }

            return View(registration);
        }

        // GET: Business/InternViews/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName");

            return View();
        }

        // POST: Business/InternViews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Email_VanLang,Post_ID,Semester_ID,CV,RegistrationDate,RegistrationModify,Business_ID,InterviewResult,InterviewResultComment,Status")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", registration.Business_ID);
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title", registration.Post_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", registration.Semester_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);
            
            return View(registration);
        }

        // GET: Business/InternViews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Registration registration = db.Registrations.Find(id);

            if (registration == null)
            {
                return HttpNotFound();
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", registration.Business_ID);
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title", registration.Post_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", registration.Semester_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);
            
            return View(registration);
        }

        // POST: Business/InternViews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Email_VanLang,Post_ID,Semester_ID,CV,RegistrationDate,RegistrationModify,Business_ID,InterviewResult,InterviewResultComment,StatusInternview,StatusRegistration")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", registration.Business_ID);
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title", registration.Post_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", registration.Semester_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);
            
            return View(registration);
        }

        // GET: Business/InternViews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Registration registration = db.Registrations.Find(id);

            if (registration == null)
            {
                return HttpNotFound();
            }

            return View(registration);
        }

        // POST: Business/InternViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = db.Registrations.Find(id);

            db.Registrations.Remove(registration);
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
