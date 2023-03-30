using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BusinessConnectManagement.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using WebGrease.Activities;
namespace BusinessConnectManagement.Areas.Business.Controllers
{
    public class InternshipResultsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Business/InternshipResults
        
        public ActionResult Index()
        {
            ViewBag.YearStudy = db.YearStudies.ToList();
            var internshipResults = db.InternshipResults.Include(i => i.BusinessUser).Include(i => i.InternshipTopic).Include(i => i.Semester).Include(i => i.VanLangUser);
            return View(internshipResults.ToList());
        }
        [HttpGet]
        public JsonResult getDataList()
        {
            int BusinessID = Convert.ToInt16(Session["BusinessID"]);
            var dataList = from internR in db.InternshipResults
                           where internR.Business_ID == BusinessID
                           select new
                           {
                               id = internR.ID,
                               name = internR.VanLangUser.FullName,
                               email = internR.Student_Email,
                               phone = internR.VanLangUser.Mobile,
                               status = internR.Status,
                               position = internR.InternshipTopic.InternshipTopicName,
                               semester = internR.Semester.Semester1,
                           };
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }
        // GET: Business/InternshipResults/Details/5
        public ActionResult Details(int id)
        {
            int BusinessID = Convert.ToInt16(Session["BusinessID"]);
            var dataList = from internR in db.InternshipResults
                           where internR.ID == id
                           select new
                           {
                               id = internR.ID,
                               Student_Email = internR.Student_Email,
                               Semester_ID = internR.Semester_ID,
                               Mentor_Email = internR.Mentor_Email,
                               MentorPoint = internR.MentorPoint,
                               MentorComment = internR.MentorComment,
                               Business_ID = internR.Business_ID,
                               BusinessPoint = internR.BusinessPoint,
                               BusinessComment = internR.BusinessComment,
                               InternshipTopic_ID = internR.InternshipTopic_ID,
                               status = internR.Status,
                               FullName = internR.VanLangUser.FullName,
                               Student_ID = internR.VanLangUser.Student_ID,
                               Mobile = internR.VanLangUser.Mobile,
                               Major = internR.VanLangUser.Major.Major1,
                               Position = internR.InternshipTopic.InternshipTopicName
                           };
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        // GET: Business/InternshipResults/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName");
            return View();
        }

        // POST: Business/InternshipResults/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Student_Email,Semester_ID,Mentor_Email,MentorPoint,MentorComment,Business_ID,BusinessPoint,BusinessComment,InternshipTopic_ID,Status")] InternshipResult internshipResult)
        {
            if (ModelState.IsValid)
            {
                db.InternshipResults.Add(internshipResult);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", internshipResult.Business_ID);
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName", internshipResult.InternshipTopic_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", internshipResult.Semester_ID);
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName", internshipResult.Student_Email);
            return View(internshipResult);
        }

        // GET: Business/InternshipResults/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InternshipResult internshipResult = db.InternshipResults.Find(id);
            if (internshipResult == null)
            {
                return HttpNotFound();
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", internshipResult.Business_ID);
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName", internshipResult.InternshipTopic_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", internshipResult.Semester_ID);
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName", internshipResult.Student_Email);
            return View(internshipResult);
        }

        // POST: Business/InternshipResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Student_Email,Semester_ID,Mentor_Email,MentorPoint,MentorComment,Business_ID,BusinessPoint,BusinessComment,InternshipTopic_ID,Status")] InternshipResult internshipResult)
        {
            if (ModelState.IsValid)
            {
                var registration = db.Registrations.Where(x => x.InternshipResult_ID == internshipResult.ID).FirstOrDefault();
                if (db.Registrations.Any(x => x.InternshipResult_ID == internshipResult.ID))
                {
                    registration.InterviewResult = "Chờ Xác Nhận";
                    db.Entry(registration).State = EntityState.Modified;
                }
                if(internshipResult.Status == "Thực Tập Xong")
                {
                    registration.InterviewResult = "Thực Tập Xong";
                    db.Entry(registration).State = EntityState.Modified;
                    string template = Server.MapPath("~/Areas/Admin/Views/Email/EmailBusinessInternship.cshtml");
                    string emailBody = System.IO.File.ReadAllText(template);

                    string To = internshipResult.Student_Email;
                    var studentName = db.InternshipResults.Where(x => x.VanLangUser.Email == internshipResult.Student_Email).Select(x => x.VanLangUser.FullName).FirstOrDefault();
                    var buName = db.InternshipResults.Where(x => x.Business_ID == internshipResult.Business_ID).Select(x => x.BusinessUser.BusinessName).FirstOrDefault();
                    string buPoint = (internshipResult.BusinessPoint).ToString();
                    emailBody = emailBody.Replace("{studentName}", studentName);
                    emailBody = emailBody.Replace("{buName}", buName);
                    emailBody = emailBody.Replace("{BusinessPoint}", buPoint);
                    emailBody = emailBody.Replace("{BusinessComment}", internshipResult.BusinessComment);
                    string Subject = "Thông Báo";
                    string Body = emailBody;
                    Outlook mail = new Outlook(To, Subject, Body);
                    mail.SendMail();
                }
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Cập Nhật Thành Công.</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
                
                db.SaveChanges();
                db.Entry(internshipResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", internshipResult.Business_ID);
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName", internshipResult.InternshipTopic_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", internshipResult.Semester_ID);
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName", internshipResult.Student_Email);
            TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">Cập Nhật Không Thành Công.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";

            return RedirectToAction("Index");

        }

        // GET: Business/InternshipResults/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InternshipResult internshipResult = db.InternshipResults.Find(id);
            if (internshipResult == null)
            {
                return HttpNotFound();
            }
            return View(internshipResult);
        }

        // POST: Business/InternshipResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InternshipResult internshipResult = db.InternshipResults.Find(id);
            db.InternshipResults.Remove(internshipResult);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExportToExcel(int semester_id)
        {
            if (ModelState.IsValid)
            {
                int BusinessID = Convert.ToInt16(Session["BusinessID"]);
                var myTable = db.InternshipResults.Where(x => x.Business_ID == BusinessID && x.Semester_ID == semester_id).ToList();
                var intern = myTable.Select(x => new InternshipResult
                {
                    VanLangUser = x.VanLangUser,
                    Student_Email = x.Student_Email,
                    Semester_ID = x.Semester_ID,
                    Mentor_Email = x.Mentor_Email,
                    MentorPoint = x.MentorPoint,
                    BusinessPoint = x.BusinessPoint,
                    BusinessComment = x.BusinessComment,
                    InternshipTopic_ID = x.InternshipTopic_ID,
                    InternshipTopic = x.InternshipTopic,
                    Status = x.Status,

                }).ToList();

                if (db.InternshipResults.Where(x => x.Business_ID == BusinessID && x.Semester_ID == semester_id).Count() != 0)
                {
                    var getValueSemester = db.InternshipResults.Where(x => x.Business_ID == BusinessID && x.Semester_ID == semester_id).FirstOrDefault();
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    using (ExcelPackage pck = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                        //customize
                        var modelTable = ws.Cells;
                        modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        ws.Cells.Style.Font.Name = "Times New Roman";
                        ws.Cells.Style.Font.Size = 12;
                        ws.Row(5).Style.Font.Size = 14;
                        ws.Row(5).Style.Font.Bold = true;


                        ws.Cells["A1"].Value = "Văn Lang University";
                        ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        ws.Cells["A2"].Value = "Business Connection Management";
                        ws.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        ws.Cells["A3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now); ;
                        ws.Row(1).Style.Font.Bold = true;
                        ws.Cells["A2"].Style.Font.Bold = true;
                        ws.Cells["A4:J4"].Merge = true;
                        ws.Cells["A4"].Value = "Danh Sách Sinh Viên Thực Tập Học Kỳ " + getValueSemester.Semester.Semester1;
                        ws.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        ws.Row(4).Style.Font.Bold = true;
                        ws.Row(4).Style.Font.Size = 18;

                        ws.Cells["A5"].Value = "Email VLU";
                        ws.Cells["B5"].Value = "Họ Và Tên";
                        ws.Cells["C5"].Value = "MSSV";
                        ws.Cells["D5"].Value = "Số Điện Thoại";
                        ws.Cells["E5"].Value = "Vị Trí Thực Tập";
                        ws.Cells["F5"].Value = "Trạng Thái";
                        ws.Cells["G5"].Value = "Điểm Thực Tập";
                        ws.Cells["H5"].Value = "Nhận Xét";
                        int rowStart = 6;
                        foreach (var item in myTable)
                        {
                            if (item.Status == "Thực Tập Xong")
                            {
                                ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#b3ffb3"));
                                ws.Row(rowStart).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                            if (item.Status == "Đang Thực Tập")
                            {
                                ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffff99"));
                                ws.Row(rowStart).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                            ws.Cells[string.Format("A{0}", rowStart)].Value = item.Student_Email;
                            ws.Cells[string.Format("B{0}", rowStart)].Value = item.VanLangUser.FullName;
                            ws.Cells[string.Format("C{0}", rowStart)].Value = item.VanLangUser.Student_ID;
                            ws.Cells[string.Format("D{0}", rowStart)].Value = item.VanLangUser.Mobile;
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.InternshipTopic.InternshipTopicName;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = item.Status;
                            ws.Cells[string.Format("G{0}", rowStart)].Value = item.BusinessPoint;
                            ws.Cells[string.Format("H{0}", rowStart)].Value = item.BusinessComment;
                            rowStart++;
                        }




                        ws.Cells["A:AZ"].AutoFitColumns();
                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=ExcelReport.xlsx");
                        Response.BinaryWrite(pck.GetAsByteArray());
                        Response.End();
                    }

                }
                TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">Học kỳ không có dữ liệu để Export</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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
            var data = db.InternshipResults.Where(x => x.Semester_ID == selectedSemesterId)
                .Select(x => new
                {
                    id = x.ID,
                }).ToList();
            return Json(new
            {
                data
            }, JsonRequestBehavior.AllowGet);
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
