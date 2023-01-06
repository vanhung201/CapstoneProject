﻿using BusinessConnectManagement.Middleware;
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
    public class CooperationCategoriesController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/CooperationCategories
        public ActionResult Index()
        {
            return View(db.CooperationCategories.ToList());
        }

        // GET: Faculty/CooperationCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CooperationCategory cooperationCategorie = db.CooperationCategories.Find(id);

            if (cooperationCategorie == null)
            {
                return HttpNotFound();
            }

            return View(cooperationCategorie);
        }

        // GET: Faculty/CooperationCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faculty/CooperationCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,CooperationCategoriesName")] CooperationCategory cooperationCategorie)
        {
            if (ModelState.IsValid)
            {
                db.CooperationCategories.Add(cooperationCategorie);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cooperationCategorie);
        }

        // GET: Faculty/CooperationCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CooperationCategory cooperationCategorie = db.CooperationCategories.Find(id);

            if (cooperationCategorie == null)
            {
                return HttpNotFound();
            }

            return View(cooperationCategorie);
        }

        // POST: Faculty/CooperationCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,CooperationCategoriesName")] CooperationCategory cooperationCategorie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cooperationCategorie).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cooperationCategorie);
        }

        // GET: Faculty/CooperationCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CooperationCategory cooperationCategorie = db.CooperationCategories.Find(id);

            if (cooperationCategorie == null)
            {
                return HttpNotFound();
            }

            return View(cooperationCategorie);
        }

        // POST: Faculty/CooperationCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CooperationCategory cooperationCategorie = db.CooperationCategories.Find(id);
           
            if (db.BusinessCooperationCategories.Where(x => x.CooperationCategories_ID == id).Any() == true)
            {
                TempData["AlertMessage"] = "Xóa không thành công vì đã có doanh nghiệp chọn hình thưc hợp tác này!!";
                
                return RedirectToAction("Index");
            }

            db.CooperationCategories.Remove(cooperationCategorie);
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