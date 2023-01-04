/*using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private BCMEntities db = new BCMEntities();
        private RoleRedirect roleRedirect = new RoleRedirect();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var query = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
            var vanLangUsers = db.VanLangUsers.Include(v => v.Major).Include(v => v.Role);

            if (query.Role_ID == "")
            {
                // Update Last Access when user login without click Login button
                query.Last_Access= DateTime.Now;

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
                
                return View(vanLangUsers.ToList());
            }
            else
            {
                return roleRedirect.Redirect(query.Role_ID.Value);
            }
        }

        public ActionResult Authorizelist()
        {
            var vanLangUsers = db.VanLangUsers.Where(x => x.Role_ID != 1);

            ViewBag.StatusList = db.Status.ToList();
            ViewBag.RoleList = db.Roles.ToList();

            return View(vanLangUsers.ToList());
        }

        public ActionResult EditUserRole(string email)
        {
            var user = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();

            if (user != null)
            {
                return Json(new { RoleId = user.Role_ID, StatusId = user.Status_ID }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [HttpPost]
        public ActionResult Edit(string StatusId, string RoleId, string email)
        {
            var user = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();

            user.Status_ID = Int32.Parse(StatusId);
            user.Role_ID = Int32.Parse(RoleId);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AuthorizeList", "Home");
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
}*/