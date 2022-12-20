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
    [Authorize]
    public class MOUsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/MOUs
        public ActionResult Index()
        {
            var mOUs = db.MOUs.Include(m => m.BusinessUser);
            ViewBag.Business_ID = db.BusinessUsers.ToList();
            return View(mOUs.ToList());
        }

        // GET: Faculty/MOUs/Details/5
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
            ViewBag.Business_ID = db.BusinessUsers.ToList();

            return View(mOU);
        }

        // GET: Faculty/MOUs/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "Business_ID", "Password");
            
            return View();
        }

        // POST: Faculty/MOUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create([Bind(Include = "ID,Business_ID,SignDay,Signer,ContactName,ContactPhone,EmailContact")] MOU mOU)
        {
            if (ModelState.IsValid)
            {
                db.MOUs.Add(mOU);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "Business_ID", "Password", mOU.Business_ID);
            
            return View(mOU);
        }

        // GET: Faculty/MOUs/Edit/5
        public ActionResult Edit(int id)
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

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "Business_ID", "Password", mOU.Business_ID);
            
            return View(mOU);
        }

        // POST: Faculty/MOUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Business_ID,SignDay,Signer,ContactName,ContactPhone,EmailContact")] MOU mOU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mOU).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Business_ID = new SelectList(db.BusinessUsers, "Business_ID", "Password", mOU.Business_ID);
                
                return View(mOU);
            }
        }

        // GET: Faculty/MOUs/Delete/5
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

        // POST: Faculty/MOUs/Delete/5
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