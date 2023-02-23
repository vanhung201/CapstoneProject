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
    public class MOUsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/MOUs
        public ActionResult Index()
        {
            var mOUs = db.MOUs.Include(m => m.BusinessUser);
            var mOU = db.MOUs.Include(m => m.BusinessUser.Registrations);

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
            ViewBag.Business_ID = db.BusinessUsers.ToList();

            return View();
        }

        // POST: Faculty/MOUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create([Bind(Include = "ID,Business_ID,SignDay,Signer,ContactName,ContactPhone,EmailContact,ResponsibleName")] MOU mOU, HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                if(db.MOUs.Where(x => x.Business_ID == mOU.Business_ID).Any() == true)
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Doanh nghiệp đã ký kết MOU.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                    return RedirectToAction("Index");
                }
                db.MOUs.Add(mOU);
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Thêm thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "Business_ID", "Password", mOU.Business_ID);

            return View(mOU);
        }

        // GET: Faculty/MOUs/Edit/5
        public ActionResult Edit(int id)
        {
           

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
        public ActionResult Edit([Bind(Include = "ID,Business_ID,SignDay,Signer,ContactName,ContactPhone,EmailContact,ResponsibleName")] MOU mOU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mOU).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
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
            TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Xóa thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
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