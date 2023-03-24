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
        public ActionResult Details(int id)
        {
          var listDetail = from intern in db.InternshipResults
                           where intern.ID== id
                           select new
                           {
                               ID = intern.ID,
                               Email = intern.Student_Email,
                               Semester_ID = intern.Semester_ID,
                               Email_Mentor = intern.Mentor_Email,
                               MentorPoint= intern.MentorPoint,
                               MentorComment = intern.MentorComment,
                               Business_ID = intern.Business_ID,
                               Businessname = intern.BusinessUser.BusinessName,
                               BusinessPoint = intern.BusinessPoint,
                               BusinessComment = intern.BusinessComment,
                               Position_ID = intern.InternshipTopic_ID,
                               Position = intern.InternshipTopic.InternshipTopicName,
                               status = intern.Status
                           };
            return Json(listDetail, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(InternshipResult internshipResult)
        {
            if (ModelState.IsValid)
            {
                var registration = db.Registrations.Where(x => x.InternshipResult_ID == internshipResult.ID).FirstOrDefault();
                if(db.Registrations.Any(x => x.InternshipResult_ID == internshipResult.ID))
                {
                    registration.InterviewResult = "Đang Thực Tập";
                    db.Entry(registration).State = EntityState.Modified;
                }
                internshipResult.Status = "Đang Thực Tập";

                if (registration.InterviewResult == "Đang Thực Tập")
                {
                    var email = db.VanLangUsers.Where(x => x.Email == internshipResult.Student_Email).First();
                    foreach (var item in db.InternshipResults)
                    {
                        if (item.Student_Email == email.Email)
                        {
                            if(internshipResult.ID != db.InternshipResults.First().ID)
                            {
                                foreach (var item2 in db.Registrations)
                                {
                                    if (item2.Email_VanLang == email.Email)
                                    {
                                        if(item2.InterviewResult == "Thực Tập Xong")
                                        {
                                            item2.InterviewResult = "Thực Tập Xong";
                                            item2.StatusInternview = "Đậu Phỏng Vấn";
                                        }
                                        else
                                        {
                                            item2.InterviewResult = "Hủy Đơn";
                                            item2.StatusInternview = "Rớt Phỏng Vấn";
                                        }
                                       
                                        db.Entry(item2).State = EntityState.Modified;
                                        registration.StatusInternview = "Đậu Phỏng Vấn";
                                        registration.InterviewResult = "Đang Thực Tập";
                                        db.Entry(registration).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        db.Entry(internshipResult).State = EntityState.Modified;
                                    }
                                }
                                if(item.Status =="Thực Tập Xong")
                                {
                                    item.Status = "Thực Tập Xong";
                                }
                                else
                                {
                                    item.Status = "Hủy Đơn";
                                }
                               
                                db.Entry(item).State = EntityState.Modified;
                                internshipResult.Status = "Đang Thực Tập";
                                db.Entry(internshipResult).State = EntityState.Modified;
                                if (item.Status == "Hủy Đơn")
                                {
                                    db.InternshipResults.Remove(item);
                                }
                            }
                            else
                            {
                                foreach (var item2 in db.Registrations)
                                {
                                    if (item2.Email_VanLang == email.Email)
                                    {
                                        if (item2.InterviewResult == "Thực Tập Xong")
                                        {
                                            item2.InterviewResult = "Thực Tập Xong";
                                            item2.StatusInternview = "Đậu Phỏng Vấn";
                                        }
                                        else
                                        {
                                            item2.InterviewResult = "Hủy Đơn";
                                            item2.StatusInternview = "Rớt Phỏng Vấn";
                                        }
                                        db.Entry(item2).State = EntityState.Modified;
                                        registration.StatusInternview = "Đậu Phỏng Vấn";
                                        registration.InterviewResult = "Đang Thực Tập";
                                        db.Entry(registration).State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        db.Entry(internshipResult).State = EntityState.Modified;
                                    }
                                }
                                if (item.Status == "Thực Tập Xong")
                                {
                                    item.Status = "Thực Tập Xong";
                                }
                                else
                                {
                                    item.Status = "Hủy Đơn";
                                }
                                db.Entry(item).State = EntityState.Modified;
                                var internR = db.InternshipResults.First();
                                internR.Status = "Đang Thực Tập";
                                db.Entry(internR).State = EntityState.Modified;
                                if (item.Status == "Hủy Đơn")
                                {
                                    db.InternshipResults.Remove(item);
                                }
                            }
                           
                        }
                        else
                        {
                            db.Entry(internshipResult).State = EntityState.Modified;
                        }
                    }
                }
                else
                {
                    if (db.InternshipResults.Any(x => x.VanLangUser.Email == registration.Email_VanLang) == false)
                    {
                        TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Cập Nhật Thành Công</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
                        return RedirectToAction("Index");
                    }
                }
              
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Cập Nhật Thành Công</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
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
