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
    public class AppointmentMentorController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/AppointmentMentor
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Mentor = db.VanLangUsers.Where(x => x.Role != "Mentor").ToList();
            var internshipResults = db.InternshipResults.Include(i => i.BusinessUser).Include(i => i.InternshipTopic).Include(i => i.Semester).Include(i => i.VanLangUser);
            return View(internshipResults.ToList());
        }
        public JsonResult getDataList()
        {
            var listData = (from intern in db.InternshipResults
                            where intern.Status != "Hủy Đơn"
                            select new
                            {
                                id = intern.ID,
                                fullname = intern.VanLangUser.FullName,
                                email = intern.Student_Email,
                                phone = intern.VanLangUser.Mobile,
                                business = intern.BusinessUser.BusinessName,
                                position = intern.InternshipTopic.InternshipTopicName,
                                status = intern.Status,
                                mentor = intern.Mentor_Email
                            });
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        // GET: Faculty/AppointmentMentor/Details/5
        public ActionResult Details(int id)
        {
            var listData = (from intern in db.InternshipResults
                            where intern.ID== id
                            select new
                            {
                                id = intern.ID,
                                email = intern.Student_Email,
                                sem_id = intern.Semester_ID,
                                mentor_email = intern.Mentor_Email,
                                mentorpoint = intern.MentorPoint,
                                mentorcomment = intern.MentorComment,
                                mentorcomment2 = intern.MentorComment2,
                                mentorcomment3 = intern.MentorCommentB1,
                                mentorcomment4 = intern.MentorCommentB2,
                                business_id = intern.Business_ID,
                                businesspoint = intern.BusinessPoint,
                                businesspoint2 = intern.BusinessPoint2,
                                businesscomment = intern.BusinessComment,
                                businesscomment2 = intern.BusinessComment2,
                                position_id = intern.InternshipTopic_ID,
                                status = intern.Status


                            });
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        // GET: Faculty/AppointmentMentor/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName");
            return View();
        }

        // POST: Faculty/AppointmentMentor/Create
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

        // GET: Faculty/AppointmentMentor/Edit/5
        public JsonResult Edit(int id)
        {
            var listData = (from intern in db.InternshipResults
                            where intern.ID == id
                            select new
                            {
                                 id = intern.ID,
                                 fullname = intern.VanLangUser.FullName,
                                 mssv = intern.VanLangUser.Student_ID,
                                 studentemail = intern.VanLangUser.Email,
                                 phone = intern.VanLangUser.Mobile,
                                 major = intern.VanLangUser.Major.Major1,
                                 position = intern.InternshipTopic.InternshipTopicName,
                                 businessName = intern.BusinessUser.BusinessName,
                                 businessMail = intern.BusinessUser.EmailContact,
                                mentor_email = intern.Mentor_Email,
                            });
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        // POST: Faculty/AppointmentMentor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Student_Email,Semester_ID,Mentor_Email,MentorPoint,MentorComment,Business_ID,BusinessPoint,BusinessPoint2,BusinessComment,InternshipTopic_ID,Status,MentorComment2,MentorCommentB1,MentorCommentB2,BusinessComment2")] InternshipResult internshipResult)
        {
            if (ModelState.IsValid)
            {
                db.Entry(internshipResult).State = EntityState.Modified;
                db.SaveChanges();
                string template = Server.MapPath("~/Areas/Admin/Views/Email/EmailFacultyMentor.cshtml");
                string emailBody = System.IO.File.ReadAllText(template);

                string To = internshipResult.Student_Email;
                var studentName = db.VanLangUsers.Where(x => x.Email == internshipResult.Student_Email).Select(x => x.FullName).FirstOrDefault();
                var mentor = db.VanLangUsers.Where(x => x.Email == internshipResult.Mentor_Email).FirstOrDefault();
                emailBody = emailBody.Replace("{studentName}", studentName);
                emailBody = emailBody.Replace("{fullname}", mentor.FullName);
                emailBody = emailBody.Replace("{Email}", internshipResult.Mentor_Email);
                emailBody = emailBody.Replace("{Mobile}", mentor.Mobile);
                string Subject = "Thông Báo";
                string Body = emailBody;
                Outlook mail = new Outlook(To, Subject, Body, "");
                mail.SendMail();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Bổ nhiệm giảng viên hướng dẫn thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";

                return RedirectToAction("Index");
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", internshipResult.Business_ID);
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName", internshipResult.InternshipTopic_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", internshipResult.Semester_ID);
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName", internshipResult.Student_Email);
            return View(internshipResult);
        }

        // GET: Faculty/AppointmentMentor/Delete/5
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

        // POST: Faculty/AppointmentMentor/Delete/5
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
