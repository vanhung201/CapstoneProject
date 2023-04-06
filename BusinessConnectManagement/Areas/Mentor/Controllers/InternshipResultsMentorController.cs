using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BusinessConnectManagement.Models;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using BusinessConnectManagement.Middleware;

namespace BusinessConnectManagement.Areas.Mentor.Controllers
{
    [LoginVerification]
    public class InternshipResultsMentorController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Mentor/InternshipResults
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.YearStudy = db.YearStudies.ToList();
            var internshipResults = db.InternshipResults.Include(i => i.BusinessUser).Include(i => i.InternshipTopic).Include(i => i.Semester).Include(i => i.VanLangUser);
            return View(internshipResults.ToList());
        }
        public JsonResult getDataList()
        {
            var email = User.Identity.Name;
            var listDataIntern = (from intern in db.InternshipResults
                            where intern.Mentor_Email == email
                            select new
                            {
                              id = intern.ID,
                              name = intern.VanLangUser.FullName,
                              phone = intern.VanLangUser.Mobile,
                              business = intern.BusinessUser.BusinessName,
                              position = intern.InternshipTopic.InternshipTopicName,
                              status = intern.Status,
                              semester_id = intern.Semester.Semester1
                            });
            return Json(listDataIntern, JsonRequestBehavior.AllowGet);
        }
        // GET: Mentor/InternshipResults/Details/5
        public ActionResult Details(int id)
        {
            var listDataIntern = (from intern in db.InternshipResults
                                  where intern.ID == id
                                  select new
                                  {
                                      id = intern.ID,
                                      name = intern.VanLangUser.FullName,
                                      mssv = intern.VanLangUser.Student_ID,
                                      email = intern.VanLangUser.Email,
                                      major = intern.VanLangUser.Major.Major1,
                                      phone = intern.VanLangUser.Mobile,
                                      position = intern.InternshipTopic.InternshipTopicName,
                                      businessname = intern.BusinessUser.BusinessName,
                                      businessmail = intern.BusinessUser.EmailContact,
                                      sem_id = intern.Semester_ID,
                                      mentor_email = intern.Mentor_Email,
                                      mentorpoint = intern.MentorPoint,
                                      mentorcomment = intern.MentorComment,
                                      business_id = intern.Business_ID,
                                      businesspoint = intern.BusinessPoint,
                                      businesscomment = intern.BusinessComment,
                                      position_id = intern.InternshipTopic_ID,
                                      status = intern.Status
                                  });
            return Json(listDataIntern, JsonRequestBehavior.AllowGet);                              
        }

        // GET: Mentor/InternshipResults/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName");
            return View();
        }

        // POST: Mentor/InternshipResults/Create
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

        // GET: Mentor/InternshipResults/Edit/5
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

        // POST: Mentor/InternshipResults/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Student_Email,Semester_ID,Mentor_Email,MentorPoint,MentorComment,Business_ID,BusinessPoint,BusinessComment,InternshipTopic_ID,Status")] InternshipResult internshipResult)
        {
            var email = User.Identity.Name;
            var curUser = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();
            var fullname = curUser.FullName;
            if (ModelState.IsValid)
            {
                db.Entry(internshipResult).State = EntityState.Modified;

                string template = Server.MapPath("~/Areas/Admin/Views/Email/EmailMentor.cshtml");
                string emailBody = System.IO.File.ReadAllText(template);

                string To = internshipResult.Student_Email;
                var studentName = db.InternshipResults.Where(x=>x.VanLangUser.Email == internshipResult.Student_Email).Select(x=>x.VanLangUser.FullName).FirstOrDefault();
                var buName = db.InternshipResults.Where(x => x.Business_ID == internshipResult.Business_ID).Select(x => x.BusinessUser.BusinessName).FirstOrDefault();
                string mentorPoint = (internshipResult.MentorPoint).ToString();
                emailBody = emailBody.Replace("{studentName}", studentName);
                emailBody = emailBody.Replace("{buName}", buName);
                emailBody = emailBody.Replace("{fullname}", fullname);
                emailBody = emailBody.Replace("{mentorPoint}", mentorPoint);
                emailBody = emailBody.Replace("{mentorComment}", internshipResult.MentorComment);
                string Subject = "Thông Báo";
                string Body = emailBody;
                Outlook mail = new Outlook(To, Subject, Body);
                mail.SendMail();
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", internshipResult.Business_ID);
            ViewBag.InternshipTopic_ID = new SelectList(db.InternshipTopics, "ID", "InternshipTopicName", internshipResult.InternshipTopic_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", internshipResult.Semester_ID);
            ViewBag.Student_Email = new SelectList(db.VanLangUsers, "Email", "FullName", internshipResult.Student_Email);
            return View(internshipResult);
        }

        // GET: Mentor/InternshipResults/Delete/5
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

        // POST: Mentor/InternshipResults/Delete/5
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
                var email = User.Identity.Name;
                var myTable = db.InternshipResults.Where(x => x.Mentor_Email == email && x.Semester_ID == semester_id).ToList();
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

                if (db.InternshipResults.Where(x => x.Mentor_Email == email && x.Semester_ID == semester_id).Count() != 0)
                {
                    var getValueSemester = db.InternshipResults.Where(x => x.Mentor_Email == email && x.Semester_ID == semester_id).FirstOrDefault();
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
                        ws.Cells["A4:L4"].Merge = true;
                        ws.Cells["A1:C1"].Merge = true;
                        ws.Cells["A2:C2"].Merge = true;
                        ws.Cells["A3:C3"].Merge = true;
                        ws.Cells["A4"].Value = "Danh Sách Sinh Viên Hướng Dẫn Học Kỳ " + getValueSemester.Semester.Semester1;
                        ws.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        ws.Row(4).Style.Font.Bold = true;
                        ws.Row(4).Style.Font.Size = 18;

                        ws.Cells["A5"].Value = "STT";
                        ws.Cells["B5"].Value = "Họ Và Tên";
                        ws.Cells["C5"].Value = "MSSV";
                        ws.Cells["D5"].Value = "Số Điện Thoại";
                        ws.Cells["E5"].Value = "Email VLU";
                        ws.Cells["F5"].Value = "Doanh Nghiệp Thực Tập";
                        ws.Cells["G5"].Value = "Vị Trí Thực Tập";
                        ws.Cells["H5"].Value = "Trạng Thái";
                        ws.Cells["I5"].Value = "Điểm Thực Tập Mentor";
                        ws.Cells["J5"].Value = "Mentor Nhận Xét";
                        ws.Cells["K5"].Value = "Điểm Thực Tập Doanh Nghiệp";
                        ws.Cells["L5"].Value = "Doanh Nghiệp Nhận Xét";
                        int rowStart = 6;
                        int countSTT = 1;
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
                            ws.Cells[string.Format("A{0}", rowStart)].Value = countSTT;
                            ws.Cells[string.Format("B{0}", rowStart)].Value = item.VanLangUser.FullName;
                            ws.Cells[string.Format("C{0}", rowStart)].Value = item.VanLangUser.Student_ID;
                            ws.Cells[string.Format("D{0}", rowStart)].Value = item.VanLangUser.Mobile;
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.Student_Email;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = item.BusinessUser.BusinessName;
                            ws.Cells[string.Format("G{0}", rowStart)].Value = item.InternshipTopic.InternshipTopicName;
                            ws.Cells[string.Format("H{0}", rowStart)].Value = item.Status;
                            ws.Cells[string.Format("I{0}", rowStart)].Value = item.MentorPoint;
                            ws.Cells[string.Format("J{0}", rowStart)].Value = item.MentorComment;
                            ws.Cells[string.Format("K{0}", rowStart)].Value = item.BusinessPoint;
                            ws.Cells[string.Format("L{0}", rowStart)].Value = item.BusinessComment;
                            rowStart++;
                            countSTT++;
                        }
                        ws.Cells["A:AZ"].AutoFitColumns();
                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=ExcelReport.xlsx");
                        Response.BinaryWrite(pck.GetAsByteArray());
                        Response.End();
                    }
                }
                else
                {
                    TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">Học kỳ không có dữ liệu để Export</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                }
               
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
