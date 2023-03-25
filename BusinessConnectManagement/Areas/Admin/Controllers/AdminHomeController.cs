using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Transactions;
using System.Web.Services.Description;

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
                sv_praticing = sv_practicing,
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

        [HttpPost]
        public ActionResult ImportData(HttpPostedFileBase postFile, VanLangUser vanLangUser)
        {
            try
            {
                String message = string.Empty;
                string path = Server.MapPath("~/Uploads/Import/" + postFile.FileName);
                if(System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                postFile.SaveAs(path);
                int count = 0;
                ImportDataEx(out count, path);
                if(ImportDataEx(out count, path) == true)
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--success\" style=\"position: abosolute; z-index: 20;\">\r\n     <div class=\"toast-left toast-left--success\">\r\n       <i class=\"fas fa-check-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n       <p class=\"toast-text\">Import dữ liệu thành công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n      <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>\r\n";
                    return RedirectToAction("AuthorizeList", "AdminHome");
                }
                else
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">File Excel không đúng định dạng dữ liệu.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                    return RedirectToAction("AuthorizeList", "AdminHome");
                }
               

            }
            catch(Exception e)
            {
                TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">File Excel không đúng định dạng dữ liệu.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                return RedirectToAction("AuthorizeList", "AdminHome");
            }
           

        }

        private bool ImportDataEx(out int count, string path)
        {
            var result = false;
            count = 0;
            try
            {
             
                using (var package = new ExcelPackage(path))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var worksheet = package.Workbook.Worksheets[0];

                    int startColumn = 1;
                    int startRow = 5;
                    //checkHeader
                    int checkColumn = 1;
                    int checkRow = 4;
                    //Student
                    object CheckEmail = worksheet.Cells[checkRow, checkColumn + 1].Value;
                    object CheckFullName = worksheet.Cells[checkRow, checkColumn + 2].Value;
                    object CheckStudent_ID = worksheet.Cells[checkRow, checkColumn + 3].Value;
                    object CheckMobile = worksheet.Cells[checkRow, checkColumn + 4].Value;
                    object CheckRole = worksheet.Cells[checkRow, checkColumn + 5].Value;
                    object Checkmajor_ID = worksheet.Cells[checkRow, checkColumn + 6].Value;
                    object Checkstatus_ID = worksheet.Cells[checkRow, checkColumn + 7].Value;
                    //mentor
                    object CheckEmailMentor = worksheet.Cells[checkRow, checkColumn + 1].Value;
                    object CheckFullNameMentor = worksheet.Cells[checkRow, checkColumn + 2].Value;
                    object CheckMobileMentor = worksheet.Cells[checkRow, checkColumn + 3].Value;
                    object CheckRoleMentor = worksheet.Cells[checkRow, checkColumn + 4].Value;
                    object Checkstatus_IDMentor = worksheet.Cells[checkRow, checkColumn + 5].Value;


                    //checkRole
                    int checkRoleColumn = 2;
                    int checkRoleRow = 2;
                    object CheckRoleStart = worksheet.Cells[checkRoleRow, checkRoleColumn].Value;
                    //
                   

                    object data = null;
                    if(CheckRoleStart.ToString()== "Mentor")
                    {
                        /*----------------------------------Mentor---------------------------*/
                        if (CheckEmailMentor.ToString() == "Email" && CheckFullNameMentor.ToString() == "Họ và tên" && CheckMobileMentor.ToString() == "SĐT" && CheckRoleMentor.ToString() == "Vai trò" && Checkstatus_IDMentor.ToString() == "Trạng thái")
                        {
                            do
                            {
                                data = worksheet.Cells[startRow, startColumn].Value;
                                object Email = worksheet.Cells[startRow, startColumn + 1].Value;
                                object FullName = worksheet.Cells[startRow, startColumn + 2].Value;
                                object Mobile = worksheet.Cells[startRow, startColumn + 3].Value;
                                object Role = worksheet.Cells[startRow, startColumn + 4].Value;
                                object status_ID = worksheet.Cells[startRow, startColumn + 5].Value;
                                //read column class name

                                if (data != null && Email != null)
                                {
                                    //importData
                                    var isSuccess = saveClassMentor(Email.ToString(), FullName.ToString(), Mobile.ToString(), Role.ToString(), Int32.Parse(status_ID.ToString()), db);
                                    if (isSuccess)
                                    {
                                        count++;
                                    }
                                }
                                startRow++;
                            }
                            while (data != null);
                            result = true;
                        }
                        else
                        {
                            result = false;
                            TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">File Excel không đúng định dạng dữ liệu.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";

                        }
                    }
                    else
                    {
                        //---------------------Student------------------
                        if (CheckEmail.ToString() == "Email" && CheckFullName.ToString() == "Họ và tên" && CheckStudent_ID.ToString() == "Mã sinh viên" && CheckMobile.ToString() == "SĐT" && CheckRole.ToString() == "Vai trò" && Checkmajor_ID.ToString() == "Chuyên nghành" && Checkstatus_ID.ToString() == "Trạng thái")
                        {
                            do
                            {
                                data = worksheet.Cells[startRow, startColumn].Value;
                                object Email = worksheet.Cells[startRow, startColumn + 1].Value;
                                object FullName = worksheet.Cells[startRow, startColumn + 2].Value;
                                object Student_ID = worksheet.Cells[startRow, startColumn + 3].Value;
                                object Mobile = worksheet.Cells[startRow, startColumn + 4].Value;
                                object Role = worksheet.Cells[startRow, startColumn + 5].Value;
                                object major_ID = worksheet.Cells[startRow, startColumn + 6].Value;
                                object status_ID = worksheet.Cells[startRow, startColumn + 7].Value;
                                //read column class name

                                if (data != null && Email != null)
                                {
                                    //importData
                                    var isSuccess = saveClass(Email.ToString(), FullName.ToString(), Student_ID.ToString(), Mobile.ToString(), Role.ToString(), Int32.Parse(major_ID.ToString()), Int32.Parse(status_ID.ToString()), db);
                                    if (isSuccess)
                                    {
                                        count++;
                                    }
                                }
                                startRow++;

                            }
                            while (data != null);
                            
                        }
                        else
                        {
                            result = false;
                        }
                    }
                   
                  
                   

                }
               
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public bool saveClassMentor(String Email, String FullName, String Mobile, String Role, int Status_ID, BCMEntities db)
        {
            var result = false;
            try
            {
              
                if(db.VanLangUsers.Where(x => x.Email.Equals(Email)).Count() == 0) 
                {
                    var item = new VanLangUser();
                    item.Email = Email;
                    item.FullName= FullName;
                    item.Mobile = Mobile;
                    item.Role = Role;
                    item.Status_ID = Status_ID;
                    db.VanLangUsers.Add(item);
                    db.SaveChanges();
                    result = true;
                }
                result = false;
            }
            catch {
                result = false;
            }
            return result; 
        }
        //-----------------------------------Mentor-----------------------
        public bool saveClass(String Email, String FullName, String Student_ID, String Mobile, String Role, int Major_ID, int Status_ID, BCMEntities db)
        {
            var result = false;
            try
            {

                if (db.VanLangUsers.Where(x => x.Email.Equals(Email)).Count() == 0)
                {
                    var item = new VanLangUser();
                    item.Email = Email;
                    item.FullName = FullName;
                    item.Student_ID = Student_ID;
                    item.Mobile = Mobile;
                    item.Role = Role;
                    item.Major_ID = Major_ID;
                    item.Status_ID = Status_ID;
                    db.VanLangUsers.Add(item);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch
            {

            }
            return result;
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