using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Mentor.Controllers
{
    public class InternshipResultsMentorController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Mentor/InternshipResults
        [HttpGet]
        public ActionResult Index()
        {
            var internshipResults = db.InternshipResults.Include(i => i.BusinessUser).Include(i => i.InternshipTopic).Include(i => i.Semester).Include(i => i.VanLangUser);
            return View(internshipResults.ToList());
        }
        public JsonResult getDataList()
        {
            var email = User.Identity.Name;
            var listDataIntern = (from intern in db.InternshipResults
                            where intern.Mentor_Email == email
                            select new
                            {
                              id = intern.ID,
                              name = intern.VanLangUser.FullName,
                              phone = intern.VanLangUser.Mobile,
                              business = intern.BusinessUser.BusinessName,
                              position = intern.InternshipTopic.InternshipTopicName,
                              status = intern.Status
                            });
            return Json(listDataIntern, JsonRequestBehavior.AllowGet);
        }
        // GET: Mentor/InternshipResults/Details/5
        public ActionResult Details(int id)
        {
            var listDataIntern = (from intern in db.InternshipResults
                                  where intern.ID == id
                                  select new
                                  {
                                      id = intern.ID,
                                      name = intern.VanLangUser.FullName,
                                      mssv = intern.VanLangUser.Student_ID,
                                      email = intern.VanLangUser.Email,
                                      major = intern.VanLangUser.Major.Major1,
                                      phone = intern.VanLangUser.Mobile,
                                      position = intern.InternshipTopic.InternshipTopicName,
                                      businessname = intern.BusinessUser.BusinessName,
                                      businessmail = intern.BusinessUser.EmailContact,
                                      sem_id = intern.Semester_ID,
                                      mentor_email = intern.Mentor_Email,
                                      mentorpoint = intern.MentorPoint,
                                      mentorcomment = intern.MentorComment,
                                      business_id = intern.Business_ID,
                                      businesspoint = intern.BusinessPoint,
                                      businesscomment = intern.BusinessComment,
                                      position_id = intern.InternshipTopic_ID,
                                      status = intern.Status
                                  });
            return Json(listDataIntern, JsonRequestBehavior.AllowGet);                              
        }

        // GET: Mentor/InternshipResults/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName");
            return View();
        }

        // POST: Mentor/InternshipResults/Create
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

        // GET: Mentor/InternshipResults/Edit/5
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

        // POST: Mentor/InternshipResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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

        // GET: Mentor/InternshipResults/Delete/5
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

        // POST: Mentor/InternshipResults/Delete/5
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
