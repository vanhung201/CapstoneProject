using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
                            join post in db.Posts on rg.Post_ID equals post.ID into postPost
                            join sem in db.Semesters on rg.Semester_ID equals sem.ID into semi
                            join email in db.VanLangUsers on rg.Email_VanLang equals email.Email into emailVL
                            select new
                            {
                                id = rg.ID,
                                username = rg.VanLangUser.FullName,
                                email = rg.VanLangUser.Email,
                                phone = rg.VanLangUser.Mobile,
                                post_id = rg.Post.Title,
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
        public ActionResult Edit([Bind(Include = "ID,Email_VanLang,Post_ID,Semester_ID,CV,RegistrationDate,RegistrationModify,Business_ID,InterviewResult,InterviewResultComment,StatusInternview_ID,StatusInternview,Comment, StatusRegistration")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { message = "Thành Công" }, JsonRequestBehavior.AllowGet);
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật thành công</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                return RedirectToAction("Index");
            }
            return Json(new { message = "Thất Bại" }, JsonRequestBehavior.AllowGet);
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
                            join email in db.VanLangUsers on rg.Email_VanLang equals email.Email into emailVL
                            select new
                            {
                                id = rg.ID,
                                username = rg.VanLangUser.FullName,
                                email = rg.VanLangUser.Email,
                                phone = rg.VanLangUser.Mobile,
                                post_id = rg.Post.Title,
                                cv = rg.CV,
                                status = rg.StatusRegistration,
                                comment = rg.Comment
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