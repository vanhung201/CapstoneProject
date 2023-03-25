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
                sv_practicing = sv_practicing,
                sv_pending = sv_pending,
                sv_completed = sv_completed,
                mentor = mentor,
                business = business,
                businessList = businessList,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BusinessFilter(int selectedBusinessId)
        {
            var posProvider = db.PostInternshipTopics
                .Where(x => x.Business_ID == selectedBusinessId)
                .Select(x => new { position = x.InternshipTopic.InternshipTopicName })
                .DistinctBy(x => x).ToArray();
            var posCount = posProvider.Select(pp => db.InternshipTopics.Count(x => x.InternshipTopicName == pp.position)).ToArray();

            return Json(new { posProvider = posProvider, posCount = posCount }, JsonRequestBehavior.AllowGet);
        }
    }
}