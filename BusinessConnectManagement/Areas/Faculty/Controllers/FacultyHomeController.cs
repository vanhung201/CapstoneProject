using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    [LoginVerification]
    public class FacultyHomeController : Controller
    {
        private BCMEntities db = new BCMEntities();
        // GET: Faculty/FacultyHome
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
            var interns = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId).Count();
            var cv_pending = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId && x.StatusRegistration == "Chờ Duyệt").Count();
            var cv_accepted = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId && x.StatusRegistration == "Phê Duyệt").Count();
            var cv_failed = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId && x.StatusRegistration == "Hủy Duyệt").Count();
            var cv_canceled = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId && x.StatusRegistration == "Không Duyệt").Count();
            var sv_failed = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId && x.StatusInternview == "Rớt Phỏng Vấn").Count();
            var sv_passed = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId && x.StatusInternview == "Đậu Phỏng Vấn").Count();
            var sv_practicing = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId && x.Status == "Đang Thực Tập").Count();
            var sv_pending = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId && x.Status == "Chờ Xác Nhận").Count();
            var sv_completed = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId && x.Status == "Thực Tập Xong").Count();
            var mentor = db.InternshipResults
                    .Where(x => x.Semester_ID == selectedSemesterId)
                    .Select(x => x.Mentor_Email)
                    .DistinctBy(x => x)
                    .Count();
            var business = db.BusinessUsers.Where(x => x.Semester_ID == selectedSemesterId).Count();
            var businessReg = db.Registrations.Where(x => x.Semester_ID == selectedSemesterId).Select(x => x.BusinessUser.BusinessName).DistinctBy(x => x).ToList();
            var businessName = db.BusinessUsers.Where(x => x.Semester_ID == selectedSemesterId).Select(x => x.BusinessName).DistinctBy(x => x).ToList();
            var buID = db.BusinessUsers.Select(x => x.ID).ToArray();
            var passStudent = buID.Select(buId => db.Registrations.Count(x => x.Business_ID == buId && x.StatusInternview == "Đậu Phỏng Vấn" && x.Semester_ID == selectedSemesterId)).DistinctBy(x => x).ToArray();
            var businessList = db.BusinessUsers.Where(x => x.Semester_ID == selectedSemesterId)
                .Select(x => new
                {
                    ID = x.ID,
                    Name = x.BusinessName
                });
            return Json(new
            {
                cv_pending = cv_pending,
                cv_accepted = cv_accepted,
                cv_failed = cv_failed,
                cv_canceled = cv_canceled,
                sv_failed = sv_failed,
                sv_passed = sv_passed,
                sv_praticing = sv_practicing,
                sv_pending = sv_pending,
                sv_completed = sv_completed,
                mentor = mentor,
                business = business,
                businessList = businessList,
                businessReg = businessReg,
                businessName = businessName,
                passStudent = passStudent
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BusinessFilter(int selectedBusinessId)
        {
            var posProvider = db.PostInternshipTopics
                .Where(x => x.Business_ID == selectedBusinessId)
                .Select(x => new { position = x.InternshipTopic.InternshipTopicName, id = x.InternshipTopic_ID })
                .DistinctBy(x => x).ToList();

            var posCount = new List<int>();
            foreach (var pos in posProvider)
            {
                int totalCount = db.PostInternshipTopics
                .Where(x => x.Business_ID == selectedBusinessId && x.InternshipTopic_ID == pos.id)
                .Sum(x => x.Quantity) ?? 0;
                posCount.Add(totalCount);
            }

            return Json(new { posProvider = posProvider, posCount = posCount }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeNotification()
        {
            var query = db.VanLangUsers.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (query.Role == "Admin" || query.Role == "Faculty")
            {
                var noti = db.Notifications.Where(x => x.Mentor_Email == null && x.Business_ID == null).ToList();
                noti.ForEach(n => n.IsRead = true);
            }
            else if (query.Role == "Mentor")
            {
                var noti = db.Notifications.Where(x => x.Mentor_Email == query.Email).ToList();
                noti.ForEach(n => n.IsRead = true);
            }
            else if (query.Role == null)
            {
                int BusinessID = Convert.ToInt16(Session["BusinessID"]);
                var noti = db.Notifications.Where(x => x.Business_ID == BusinessID).ToList();
                noti.ForEach(n => n.IsRead = true);
            }
            else
            {
                return Json(new { message = "failed" }, JsonRequestBehavior.AllowGet);
            }
            db.SaveChanges();
            return Json(new { message = "successed" }, JsonRequestBehavior.AllowGet);

        }
    }
}