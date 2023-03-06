using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Admin.Controllers
{
    [AdminVerification]
    public class AdminHomeController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Admin/AdminHome
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult getDataList()
        {
            var listData = (from vl in db.VanLangUsers
                            join mj in db.Majors on vl.Major_ID equals mj.ID into major
                            join st in db.Status on vl.Status_ID equals st.ID into status
                            select new
                            {
                                email = vl.Email,
                                username = vl.FullName,
                                student = vl.Student_ID,
                                phone = vl.Mobile,
                                majorr = vl.Major.Major1,
                                access = vl.Last_Access,
                                role = vl.Role,
                                status = vl.Status.Status1
                            });
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult AuthorizeList()
        {
            var vanLangUsers = db.VanLangUsers.Where(x => x.Role != "Admin");

            ViewBag.StatusList = db.Status.ToList();

            return View(vanLangUsers.ToList());
        }
        [HttpGet]
        public ActionResult Details(string email)
        {
            VanLangUser vanLangUser = db.VanLangUsers.Find(email);

            var list = (from vl in db.VanLangUsers
                            join mj in db.Majors on vl.Major_ID equals mj.ID into major
                            join st in db.Status on vl.Status_ID equals st.ID into status
                            where vl.Email == email
                            select new
                            {
                                email= vl.Email,
                                username = vl.FullName,
                                student = vl.Student_ID,
                                phone = vl.Mobile,
                                majorr = vl.Major.Major1,
                                access = vl.Last_Access,
                                role = vl.Role,
                                status = vl.Status.Status1
                            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(string email)
        {
           

            VanLangUser vanLangUser = db.VanLangUsers.Find(email);

            var listData = (from vl in db.VanLangUsers
                            join mj in db.Majors on vl.Major_ID equals mj.ID into major
                            join st in db.Status on vl.Status_ID equals st.ID into status
                            where vl.Email == email
                            select new
                            {
                                email = vl.Email,
                                username = vl.FullName,
                                phone = vl.Mobile,
                                role = vl.Role,
                                status = vl.Status.Status1
                            });
            return Json(listData, JsonRequestBehavior.AllowGet);

         
        }
        
        [HttpPost]
        public ActionResult Edit(VanLangUser vanLangUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vanLangUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\" style=\"position: abosolute; z-index: 20;\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Cập nhật thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                return RedirectToAction("AuthorizeList");
            }

            return View(vanLangUser);
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