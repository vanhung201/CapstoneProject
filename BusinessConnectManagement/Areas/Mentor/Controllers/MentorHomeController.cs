using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Mentor.Controllers
{
    [LoginVerification]
    public class MentorHomeController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Mentor/Home
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
            var email = User.Identity.Name;
            var sv_practicing = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId &&
            x.Status == "Đang Thực Tập" &&
            x.Mentor_Email == email).Count();
            var sv_completed = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId &&
            x.Status == "Thực Tập Xong" &&
            x.Mentor_Email == email).Count();
            return Json(new
            {
                sv_practicing = sv_practicing,
                sv_completed = sv_completed,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeNotification()
        {
            var query = db.VanLangUsers.FirstOrDefault(x => x.Email == User.Identity.Name);
            
                var noti = db.Notifications.Where(x => x.Mentor_Email == query.Email).ToList();
                noti.ForEach(n => n.IsRead = true);
            
            db.SaveChanges();
            return Json(new { message = "successed" }, JsonRequestBehavior.AllowGet);

        }
    }
}