using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    [LoginVerification]
    public class RegistrationController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Manager/Registration
        [HttpGet]
        public ActionResult Index()
        {
            var registration = db.Registrations.ToList();

            return View(registration);
        }
        public JsonResult getDataList()
        {
            var listData = (from rg in db.Registrations
                            join bu in db.BusinessUsers on rg.Business_ID equals bu.ID into business
                            join post in db.Posts on rg.Post_ID equals post.ID 
                            join sem in db.Semesters on rg.Semester_ID equals sem.ID into semi
                            join emailVL in db.VanLangUsers on rg.Email_VanLang equals emailVL.Email
                            orderby rg.StatusRegistration == "Chờ Duyệt" descending
                            select new
                            {
                                id = rg.ID,
                                username = emailVL.FullName,
                                email = emailVL.Email,
                                phone = emailVL.Mobile,
                                post_id = post.Title,
                                cv = rg.CV,
                                status = rg.StatusRegistration,
                                comment = rg.Comment
                            });
            return Json(listData,   JsonRequestBehavior.AllowGet);
        }
      
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

            return View(registration);
        }
        
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Email_VanLang,Post_ID,Semester_ID,CV,RegistrationDate,RegistrationModify,Business_ID,InterviewResult,InterviewResultComment,StatusInternview_ID,StatusInternview,Comment, StatusRegistration, InternshipTopic_ID")] Registration registration)
        {
            var post = db.Posts.Where(x => x.ID == registration.Post_ID).First();
            var email = db.VanLangUsers.Where(x=>x.Email == registration.Email_VanLang).First();
            Notification notify = new Notification();
            if (ModelState.IsValid)
            {
                /*if (db.Registrations.Any(x => x.Email_VanLang == email.Email && x.StatusInternview == "Đậu"))
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--success\"> <div class=\"toast-left toast-left--success\"> <i class=\"fas fa-check-circle\"></i>\r\n  </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Cập Nhật Không Thành Công Vì Sinh Viên Đã Đậu Phỏng Vấn</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
                    return RedirectToAction("Index");
                }*/
                registration.StatusInternview = "Chờ Phỏng Vấn";
                db.Entry(registration).State = EntityState.Modified;
                notify.Business_ID = registration.Business_ID;
                notify.Title = "Thông báo";
                notify.Message = "Có sinh viên vừa ứng tuyển vào công ty bạn";
                notify.IsRead = false;
                notify.Date = (DateTime.Now).ToString();
                notify.Link = Url.Action("Index", "InternViews", new { area = "Business" });
                db.Notifications.Add(notify);
                db.SaveChanges();
                string template = Server.MapPath("~/Areas/Admin/Views/Email/EmailFacultyCV.cshtml");
                string emailBody = System.IO.File.ReadAllText(template);

                string To = registration.Email_VanLang;
                var studentName = db.VanLangUsers.Where(x => x.Email == registration.Email_VanLang).Select(x => x.FullName).FirstOrDefault();
                var buName = db.BusinessUsers.Where(x => x.ID == registration.Business_ID).Select(x => x.BusinessName).FirstOrDefault();
                emailBody = emailBody.Replace("{studentName}", studentName);
                emailBody = emailBody.Replace("{buName}", buName);
                emailBody = emailBody.Replace("{StatusRegistration}", registration.StatusRegistration);
                emailBody = emailBody.Replace("{Comment}", registration.Comment);
                string Subject = "Thông Báo";
                string Body = emailBody;
                Outlook mail = new Outlook(To, Subject, Body, "");
                mail.SendMail();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật thành công</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                return RedirectToAction("Index");
            }
        
            return View(registration);
        }

        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~/Uploads/CV/" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public ActionResult Details(int id)
        {
           
            Registration registration = db.Registrations.Find(id);

            var listData = (from rg in db.Registrations
                            join bu in db.BusinessUsers on rg.Business_ID equals bu.ID into business
                            join post in db.Posts on rg.Post_ID equals post.ID into postPost
                            join sem in db.Semesters on rg.Semester_ID equals sem.ID into semi
                            join emailVL in db.VanLangUsers on rg.Email_VanLang equals emailVL.Email 
                            where rg.ID == id
                            select new
                            {
                                id = rg.ID,
                                email = emailVL.Email,
                                post_id = rg.Post_ID,
                                semester_id = rg.Semester_ID,
                                cv = rg.CV,
                                registrationDate = rg.RegistrationDate,
                                registrationModify = rg.RegistrationModify,
                                bu_id = rg.Business_ID,
                                interviewResult = rg.InterviewResult,
                                interviewComment = rg.InterviewResultComment,
                                interviewStatus = rg.StatusInternview,
                                status = rg.StatusRegistration,
                                comment = rg.Comment,
                                username = emailVL.FullName,
                                phone = emailVL.Mobile,
                                position_id = rg.InternshipTopic_ID,
                                major = rg.VanLangUser.Major.Major1,
                                buname = rg.BusinessUser.BusinessName,
                                student_id = rg.VanLangUser.Student_ID,
                                mobile = rg.VanLangUser.Mobile,
                                position = rg.InternshipTopic.InternshipTopicName,
                                bumail = rg.BusinessUser.EmailContact
                            });
            return Json(listData, JsonRequestBehavior.AllowGet);
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