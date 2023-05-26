using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Business.Controllers
{
    [BusinessVerification]
    public class BusinessHomeController : Controller
    {
        private BCMEntities db = new BCMEntities();
        // GET: Business/Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult BindingSemester(int selectedYearId)
        {
            var semesters = db.Semesters
                .Where(s => s.YearStudy_ID == selectedYearId)
                .Select(s => new {
                    ID = s.ID,
                    Semester = s.Semester1,
                    Status = s.Status
                })
                .ToList();
            return Json(semesters, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SemesterFilter(int selectedSemesterId)
        {
            int BusinessID = Convert.ToInt16(Session["BusinessID"]);
            var sv_failed = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId && 
            x.StatusInternview == "Rớt Phỏng Vấn" && 
            x.Business_ID == BusinessID).Count();
            var sv_passed = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId &&
            x.StatusInternview == "Đậu Phỏng Vấn" &&
            x.Business_ID == BusinessID).Count();
            var sv_practicing = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId &&
            x.Status == "Đang Thực Tập" &&
            x.Business_ID == BusinessID).Count();
            var sv_pending = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId &&
            x.Status == "Chờ Xác Nhận" &&
            x.Business_ID == BusinessID).Count();
            var sv_completed = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId &&
            x.Status == "Thực Tập Xong" &&
            x.Business_ID == BusinessID).Count();
            return Json(new
            {
                sv_failed = sv_failed,
                sv_passed = sv_passed,
                sv_practicing = sv_practicing,
                sv_pending = sv_pending,
                sv_completed = sv_completed,
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeNotification()
        {
            
            int BusinessID = Convert.ToInt16(Session["BusinessID"]);
            var noti = db.Notifications.Where(x => x.Business_ID == BusinessID).ToList();
            noti.ForEach(n => n.IsRead = true);
            db.SaveChanges();
            return Json(new { message = "successed" }, JsonRequestBehavior.AllowGet);

        }
    }
}