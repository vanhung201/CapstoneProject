using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    public class RegistrationsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/Registrations
        public ActionResult Index()
        {
            var registrations = db.Registrations.Include(r => r.BusinessUser).Include(r => r.Post).Include(r => r.Semester).Include(r => r.StatusInternview).Include(r => r.VanLangUser);
            return View(registrations.ToList());
        }

        // GET: Faculty/Registrations/Details/5
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
            ViewBag.Registration = registration;
            ViewBag.FullName = registration.VanLangUser.FullName;
            return View(registration);
        }

        // GET: Faculty/Registrations/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.StatusInternview_ID = new SelectList(db.StatusInternviews, "ID", "Status");
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName");
            return View();
        }

        // POST: Faculty/Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Email_VanLang,Post_ID,Semester_ID,CV,RegistrationDate,RegistrationModify,Business_ID,InterviewResult,InterviewResultComment,StatusInternview_ID,RegistrationPosition")] Registration registration)
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
            ViewBag.StatusInternview_ID = new SelectList(db.StatusInternviews, "ID", "Status", registration.StatusInternview_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);
            return View(registration);
        }

        // GET: Faculty/Registrations/Edit/5
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
            ViewBag.StatusInternview_ID = new SelectList(db.StatusInternviews, "ID", "Status", registration.StatusInternview_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);
            return View(registration);
        }

        // POST: Faculty/Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Email_VanLang,Post_ID,Semester_ID,CV,RegistrationDate,RegistrationModify,Business_ID,InterviewResult,InterviewResultComment,StatusInternview_ID,RegistrationPosition")] Registration registration)
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
            ViewBag.StatusInternview_ID = new SelectList(db.StatusInternviews, "ID", "Status", registration.StatusInternview_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);
            return View(registration);
        }

        // GET: Faculty/Registrations/Delete/5
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

        // POST: Faculty/Registrations/Delete/5
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
