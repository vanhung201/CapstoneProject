using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Controllers
{
    public class MOUsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: MOUs
        public ActionResult Index()
        {
            var mOUs = db.MOUs.Include(m => m.BusinessUser);
            return View(mOUs.ToList());
        }

        // GET: MOUs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOU mOU = db.MOUs.Find(id);
            if (mOU == null)
            {
                return HttpNotFound();
            }
            ViewBag.Post = db.Posts.Where(x => x.Business_ID == mOU.Business_ID).ToList();
            return View(mOU);
        }

        // GET: MOUs/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            return View();
        }

        // POST: MOUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Business_ID,SignDay,Signer,ContactName,ContactPhone,EmailContact,ResponsibleName")] MOU mOU)
        {
            if (ModelState.IsValid)
            {
                db.MOUs.Add(mOU);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", mOU.Business_ID);
            return View(mOU);
        }

        // GET: MOUs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOU mOU = db.MOUs.Find(id);
            if (mOU == null)
            {
                return HttpNotFound();
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", mOU.Business_ID);
            return View(mOU);
        }

        // POST: MOUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Business_ID,SignDay,Signer,ContactName,ContactPhone,EmailContact,ResponsibleName")] MOU mOU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mOU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", mOU.Business_ID);
            return View(mOU);
        }

        // GET: MOUs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MOU mOU = db.MOUs.Find(id);
            if (mOU == null)
            {
                return HttpNotFound();
            }
            return View(mOU);
        }

        // POST: MOUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MOU mOU = db.MOUs.Find(id);
            db.MOUs.Remove(mOU);
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
