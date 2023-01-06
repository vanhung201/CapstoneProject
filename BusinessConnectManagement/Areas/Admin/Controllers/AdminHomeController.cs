using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Admin.Controllers
{
    [AdminVerification]
    public class AdminHomeController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Admin/AdminHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthorizeList()
        {
            var vanLangUsers = db.VanLangUsers.Where(x => x.Role != "Admin");

            ViewBag.StatusList = db.Status.ToList();

            return View(vanLangUsers.ToList());
        }

        public ActionResult Edit(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VanLangUser vanLangUser = db.VanLangUsers.Find(email);

            if (vanLangUser == null)
            {
                return HttpNotFound();
            }

            return View(vanLangUser);
        }
        
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Email, FullName, Student_ID, Mobile, Role, Last_Access, Major_ID, Status_ID")] VanLangUser vanLangUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vanLangUser).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("AuthorizeList");
            }

            return View(vanLangUser);
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