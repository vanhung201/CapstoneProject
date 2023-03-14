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
    public class InternshipResultsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: InternshipResults
        public ActionResult Index()
        {
            var internshipResults = db.InternshipResults.Include(i => i.BusinessUser).Include(i => i.InternshipTopic).Include(i => i.Semester).Include(i => i.VanLangUser);
            return View(internshipResults.ToList());
        }
        public JsonResult getDataList()
        {
            var email = User.Identity.Name;
            var listData = from intern in db.InternshipResults
                           where intern.Student_Email == email
                           select new
                           {
                               ID = intern.ID,
                               BusinessName = intern.BusinessUser.BusinessName,
                               Position = intern.InternshipTopic.InternshipTopicName,
                               status = intern.Status
                           };
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
        // GET: InternshipResults/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InternshipResult internshipResult = db.InternshipResults.Find(id);
            if (internshipResult == null)
            {
                return HttpNotFound();
            }
            return View(internshipResult);
        }

        // GET: InternshipResults/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName");
            return View();
        }

        // POST: InternshipResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Student_Email,Semester_ID,Mentor_Email,MentorPoint,MentorComment,Business_ID,BusinessPoint,BusinessComment,InternshipTopic_ID,Status")] InternshipResult internshipResult)
        {
            if (ModelState.IsValid)
            {
                db.InternshipResults.Add(internshipResult);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", internshipResult.Business_ID);
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName", internshipResult.InternshipTopic_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", internshipResult.Semester_ID);
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName", internshipResult.Student_Email);
            return View(internshipResult);
        }

        // GET: InternshipResults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InternshipResult internshipResult = db.InternshipResults.Find(id);
            if (internshipResult == null)
            {
                return HttpNotFound();
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", internshipResult.Business_ID);
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName", internshipResult.InternshipTopic_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", internshipResult.Semester_ID);
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName", internshipResult.Student_Email);
            return View(internshipResult);
        }

        // POST: InternshipResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Student_Email,Semester_ID,Mentor_Email,MentorPoint,MentorComment,Business_ID,BusinessPoint,BusinessComment,InternshipTopic_ID,Status")] InternshipResult internshipResult)
        {
            if (ModelState.IsValid)
            {
                db.Entry(internshipResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", internshipResult.Business_ID);
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName", internshipResult.InternshipTopic_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", internshipResult.Semester_ID);
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName", internshipResult.Student_Email);
            return View(internshipResult);
        }

        // GET: InternshipResults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InternshipResult internshipResult = db.InternshipResults.Find(id);
            if (internshipResult == null)
            {
                return HttpNotFound();
            }
            return View(internshipResult);
        }

        // POST: InternshipResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InternshipResult internshipResult = db.InternshipResults.Find(id);
            db.InternshipResults.Remove(internshipResult);
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
