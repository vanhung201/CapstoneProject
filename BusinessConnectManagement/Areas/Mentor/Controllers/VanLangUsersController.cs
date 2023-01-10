using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Mentor.Controllers
{
    public class VanLangUsersController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Mentor/VanLangUsers
        public ActionResult Index()
        {
            var vanLangUsers = db.VanLangUsers.Include(v => v.Major).Include(v => v.Status);
            return View(vanLangUsers.ToList());
        }

        // GET: Mentor/VanLangUsers/Details/5
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

        // GET: Mentor/VanLangUsers/Create
        public ActionResult Create()
        {
            ViewBag.Major_ID = new SelectList(db.Majors, "ID", "Major1");
            ViewBag.Status_ID = new SelectList(db.Status, "ID", "Status1");
            return View();
        }

        // POST: Mentor/VanLangUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email,FullName,Student_ID,Mobile,Role,Last_Access,Major_ID,Status_ID")] VanLangUser vanLangUser)
        {
            if (ModelState.IsValid)
            {
                db.VanLangUsers.Add(vanLangUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Major_ID = new SelectList(db.Majors, "ID", "Major1", vanLangUser.Major_ID);
            ViewBag.Status_ID = new SelectList(db.Status, "ID", "Status1", vanLangUser.Status_ID);
            return View(vanLangUser);
        }

        // GET: Mentor/VanLangUsers/Edit/5
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
            ViewBag.Status_ID = new SelectList(db.Status, "ID", "Status1", vanLangUser.Status_ID);
            return View(vanLangUser);
        }

        // POST: Mentor/VanLangUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,FullName,Student_ID,Mobile,Role,Last_Access,Major_ID,Status_ID")] VanLangUser vanLangUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vanLangUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Major_ID = new SelectList(db.Majors, "ID", "Major1", vanLangUser.Major_ID);
            ViewBag.Status_ID = new SelectList(db.Status, "ID", "Status1", vanLangUser.Status_ID);
            return View(vanLangUser);
        }

        // GET: Mentor/VanLangUsers/Delete/5
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

        // POST: Mentor/VanLangUsers/Delete/5
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
