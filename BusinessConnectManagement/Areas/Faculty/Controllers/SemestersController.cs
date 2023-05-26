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
using Microsoft.Ajax.Utilities;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    public class SemestersController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Faculty/Semesters
        public ActionResult Index()
        {
            var semesters = db.Semesters.Include(s => s.YearStudy);
            return View(semesters.ToList());
        }

        // GET: Faculty/Semesters/Details/5
        public ActionResult Details(int? id)
        {
            var getListSemester = from sem in db.Semesters
                                  where sem.ID == id
                                  select new
                                  
                                  {
                                      id = sem.ID,
                                      year_id = sem.YearStudy_ID,
                                      name = sem.Semester1,
                                      startDate = sem.StartDate,
                                      endDate = sem.EndDate,
                                      status = sem.Status,
                                  };
            return Json(getListSemester, JsonRequestBehavior.AllowGet);
        }

        // GET: Faculty/Semesters/Create
        public ActionResult Create()
        {
            ViewBag.YearStudy_ID = new SelectList(db.YearStudies, "ID", "YearStudy1");
            return View();
        }

        // POST: Faculty/Semesters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create([Bind(Include = "ID,YearStudy_ID,Semester1,Status, StartDate, EndDate")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                db.Semesters.Add(semester);
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Thêm học kỳ thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";

                return RedirectToAction("Details", "YearStudies", new { id = semester.YearStudy_ID });

            }

            ViewBag.YearStudy_ID = new SelectList(db.YearStudies, "ID", "YearStudy1", semester.YearStudy_ID);
            return View(semester);
        }

        // GET: Faculty/Semesters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            ViewBag.YearStudy_ID = new SelectList(db.YearStudies, "ID", "YearStudy1", semester.YearStudy_ID);
            return View(semester);
        }

        // POST: Faculty/Semesters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Semester semester, bool status)
        {

            if (ModelState.IsValid)
            {
                if (semester.Status == true)
                {
                    if (semester.ID != db.Semesters.First().ID)
                    {
                        foreach (var item in db.Semesters)
                        {
                                item.Status = false;
                                db.Entry(item).State = EntityState.Modified;
                                semester.Status = status;
                                db.Entry(semester).State = EntityState.Modified;
                            
                        }
                    }
                    else
                    {
                        foreach (var item in db.Semesters)
                        {
                            item.Status = false;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        var sem = db.Semesters.First();
                        sem.Status = status;
                        db.Entry(sem).State = EntityState.Modified;
                       
                    }

                    }
                else
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Điều chỉnh học kỳ hiện tại thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                    return RedirectToAction("Index");
                }
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Điều chỉnh học kỳ hiện tại thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                return RedirectToAction("Details", "YearStudies", new {id = semester.YearStudy_ID});
            }
            ViewBag.YearStudy_ID = new SelectList(db.YearStudies, "ID", "YearStudy1", semester.YearStudy_ID);
            return View(semester);
        }
        [HttpPost]
        public ActionResult Update([Bind(Include = "ID,YearStudy_ID,Semester1,Status, StartDate, EndDate")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                db.Entry(semester).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật học kỳ thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";

                return RedirectToAction("Details", "YearStudies", new { id = semester.YearStudy_ID });
            }
            return View(semester);
        }
        // GET: Faculty/Semesters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Semester semester = db.Semesters.Find(id);
            if (semester == null)
            {
                return HttpNotFound();
            }
            return View(semester);
        }

        // POST: Faculty/Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Semester semester = db.Semesters.Find(id);
            db.Semesters.Remove(semester);
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
