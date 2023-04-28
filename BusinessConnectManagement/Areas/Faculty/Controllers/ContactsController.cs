using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    [LoginVerification]
    public class ContactsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }
        public JsonResult getData()
        {
            var getDataList = (from contact in db.Contacts
                               select new
                               {
                                   id = contact.ID,
                                   fullname = contact.FullName,
                                   mobile = contact.Mobile,
                                   email = contact.Email,
                                   note = contact.Note,
                               });
            return Json(getDataList, JsonRequestBehavior.AllowGet);
        }
        // GET: Faculty/Contacts/Details/5
        public ActionResult Details(int id)
        {
            var getDataList = (from contact in db.Contacts
                               where contact.ID == id
                               select new
                               {
                                   id = contact.ID,
                                   fullname = contact.FullName,
                                   mobile = contact.Mobile,
                                   email = contact.Email,
                                   note = contact.Note,
                               });
            return Json(getDataList, JsonRequestBehavior.AllowGet);
        }

        // GET: Faculty/Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faculty/Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FullName,Mobile,Email,Note")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Faculty/Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Faculty/Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,Mobile,Email,Note")] Contact contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Faculty/Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Faculty/Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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
