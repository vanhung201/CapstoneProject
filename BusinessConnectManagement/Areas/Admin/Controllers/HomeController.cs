using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            var vanLangUsers = db.VanLangUsers.Include(v => v.Major).Include(v => v.Role);
            return View(vanLangUsers.ToList());
        }

        public ActionResult Authorizelist()
        {
            var vanLangUsers = db.VanLangUsers.Where(x => x.Role_ID != 1);
            return View(vanLangUsers.ToList());
        }

        public ActionResult EditUserRole(string email)
        {

            var userRoles = db.Roles.Where(x => x.VanLangUsers.Any(a => a.Email == email)).OrderBy(x => x.ID).ToList();
            var rolelist = db.Roles.Where(x => x.ID != 1).ToList();
            List<object> ReturnData = new List<object>();
            foreach (var role in rolelist)
            {
                bool isExist = userRoles.Any(x => x.ID == role.ID);
                if (!isExist)
                {
                    ReturnData.Add(new { Id = role.ID, Name = role.Role_Name, Selected = false });
                }
                else
                {
                    ReturnData.Add(new { Id = role.ID, Name = role.Role_Name, Selected = true });
                }
            }
            return Json(ReturnData, JsonRequestBehavior.AllowGet);

        }

        // GET: Admin/Home/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VanLangUser vanLangUser = db.VanLangUsers.Find(id);
            if (vanLangUser == null)
            {
                return HttpNotFound();
            }
            return View(vanLangUser);
        }

        // GET: Admin/Home/Create
        public ActionResult Create()
        {
            ViewBag.Major_ID = new SelectList(db.Majors, "ID", "Major1");
            ViewBag.Role_ID = new SelectList(db.Roles, "ID", "Role_Name");
            return View();
        }

        // POST: Admin/Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,FullName,Student_ID,Role_ID,Last_Access,Major_ID,Status")] VanLangUser vanLangUser)
        {
            if (ModelState.IsValid)
            {
                db.VanLangUsers.Add(vanLangUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Major_ID = new SelectList(db.Majors, "ID", "Major1", vanLangUser.Major_ID);
            ViewBag.Role_ID = new SelectList(db.Roles, "ID", "Role_Name", vanLangUser.Role_ID);
            return View(vanLangUser);
        }

        // GET: Admin/Home/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VanLangUser vanLangUser = db.VanLangUsers.Find(id);
            if (vanLangUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.Major_ID = new SelectList(db.Majors, "ID", "Major1", vanLangUser.Major_ID);
            ViewBag.Role_ID = new SelectList(db.Roles, "ID", "Role_Name", vanLangUser.Role_ID);
            return View(vanLangUser);
        }

        // POST: Admin/Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FullName,Student_ID,Role_ID,Last_Access,Major_ID,Status")] VanLangUser vanLangUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vanLangUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Major_ID = new SelectList(db.Majors, "ID", "Major1", vanLangUser.Major_ID);
            ViewBag.Role_ID = new SelectList(db.Roles, "ID", "Role_Name", vanLangUser.Role_ID);
            return View(vanLangUser);
        }

        // GET: Admin/Home/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VanLangUser vanLangUser = db.VanLangUsers.Find(id);
            if (vanLangUser == null)
            {
                return HttpNotFound();
            }
            return View(vanLangUser);
        }

        // POST: Admin/Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            VanLangUser vanLangUser = db.VanLangUsers.Find(id);
            db.VanLangUsers.Remove(vanLangUser);
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
