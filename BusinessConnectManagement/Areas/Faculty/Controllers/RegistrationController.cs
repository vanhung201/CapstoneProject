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
        public ActionResult Index()
        {
            var registration = db.Registrations.ToList();

            return View(registration);
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