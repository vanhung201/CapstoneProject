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
    public class YearStudiesController : Controller
    {
        private BCMEntities db = new BCMEntities();
        [HttpGet]
        // GET: Faculty/YearStudies
        public ActionResult Index()
        {
            return View(db.YearStudies.ToList());
        }

        public JsonResult getDataList()
        {

            var getListSemester = from sem in db.YearStudies
                                  select new
                                  {
                                      id = sem.ID,
                                      name = sem.YearStudy1,
                                      startDate = sem.StartDate,
                                      endDate = sem.EndDate,
                                      status = sem.Status,
                                  };
            return Json(getListSemester, JsonRequestBehavior.AllowGet);
        }
        // GET: Faculty/YearStudies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearStudy yearStudy = db.YearStudies.Find(id);
            ViewBag.Semester = db.Semesters.Where(x => x.YearStudy.ID == id);
            if (yearStudy == null)
            {
                return HttpNotFound();
            }
            return View(yearStudy);
        }

        // GET: Faculty/YearStudies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faculty/YearStudies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public ActionResult Create(YearStudy yearStudy)
        {
            if (ModelState.IsValid)
            {
                if (db.YearStudies.Any(x => x.YearStudy1 == yearStudy.YearStudy1) == true)
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Đã tồn tại năm học.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";

                    return RedirectToAction("Index");
                } else
                {
                db.YearStudies.Add(yearStudy);
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Thêm thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("Index");

                }
            }

            return View(yearStudy);
        }

        // GET: Faculty/YearStudies/Edit/5
        public ActionResult Edit(int id)
        {
            var getListSemester = from sem in db.YearStudies
                                  where sem.ID == id
                                  select new
                                  {
                                      id = sem.ID,
                                      name = sem.YearStudy1,
                                      startDate = sem.StartDate,
                                      endDate = sem.EndDate,
                                      status = sem.Status
                                  };

            var formattedList = getListSemester.ToList().Select(sem => new
            {
                id = sem.id,
                name = sem.name,
                startDate = ((DateTime)sem.startDate).ToString("yyyy-MM-dd"), // Convert to desired date format
                endDate = ((DateTime)sem.endDate).ToString("yyyy-MM-dd"), // Convert to desired date format
                status = sem.status
            });

            return Json(formattedList, JsonRequestBehavior.AllowGet);
        }

        // POST: Faculty/YearStudies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,YearStudy1, StartDate, EndDate, Status")] YearStudy yearStudy)
        {
            if (ModelState.IsValid)
            {
                if(db.YearStudies.Any(x=> x.ID != yearStudy.ID && x.YearStudy1 == yearStudy.YearStudy1) == true)
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Đã tồn tại năm học.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";

                    return RedirectToAction("Index");
                }
                else
                {
                yearStudy.Status = false;
                db.Entry(yearStudy).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";

                }

                return RedirectToAction("Index");
            }
            return View(yearStudy);
        }

        // GET: Faculty/YearStudies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            YearStudy yearStudy = db.YearStudies.Find(id);
            if (yearStudy == null)
            {
                return HttpNotFound();
            }
            return View(yearStudy);
        }

        // POST: Faculty/YearStudies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            YearStudy yearStudy = db.YearStudies.Find(id);
            db.YearStudies.Remove(yearStudy);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Update(YearStudy yearStudy)
        {
            if (ModelState.IsValid)
            {
                var firstSem = db.Semesters.Where(x => x.YearStudy_ID == yearStudy.ID).FirstOrDefault();
                if(yearStudy.Status == true) {
                        foreach(var item in db.YearStudies.Where(x=>x.ID != yearStudy.ID))
                        {
                            item.Status = false;
                            db.Entry(item).State = EntityState.Modified;
                        }
                        yearStudy.Status = true;
                        db.Entry(yearStudy).State = EntityState.Modified;
                        foreach(var sem in db.Semesters.Where(x=>x.YearStudy_ID != yearStudy.ID))
                        {
                            sem.Status = false;
                            db.Entry(sem).State = EntityState.Modified;
                        }
                        if(firstSem != null)
                        {
                        firstSem.YearStudy_ID = yearStudy.ID;
                        firstSem.Status = true;
                        db.Entry(firstSem).State = EntityState.Modified;
                        }
                    else
                        {
                           Semester semester = new Semester();
                           semester.ID = yearStudy.ID;
                           semester.Semester1 = "Semester";
                           semester.StartDate = DateTime.Now;
                           semester.EndDate = DateTime.Now.AddDays(105);
                           semester.Status = true;
                        db.Semesters.Add(semester);
                        }
                    db.SaveChanges();
                    TempData["AlertMessage"] = "<div class=\"toast toast--success\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Điều chỉnh năm học hiện tại thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                    return RedirectToAction("Index");
                }else
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Điều chỉnh năm học hiện tại thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                    return RedirectToAction("Index");
                }
            }
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
