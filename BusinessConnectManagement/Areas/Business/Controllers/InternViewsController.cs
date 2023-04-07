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
using System.Web.Services.Description;
using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace BusinessConnectManagement.Areas.Business.Controllers
{
    [BusinessVerification]
    public class InternViewsController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Business/InternViews
        [HttpGet]
        public ActionResult Index()
        {
            int BusinessID = Convert.ToInt16(Session["BusinessID"]);
            ViewBag.YearStudy = db.YearStudies.ToList();
            var registrations = db.Registrations.Where(x => x.Business_ID == BusinessID && x.StatusRegistration == "Phê duyệt");

            return View(registrations.ToList());
        }

        public JsonResult getDataList()
        {
            int BusinessID = Convert.ToInt16(Session["BusinessID"]);
            var dataRegis = from re in db.Registrations
                            join post in db.Posts on re.Post_ID equals post.ID into posts
                            join sem in db.Semesters on re.Semester_ID equals sem.ID into sems
                            join bu in db.BusinessUsers on re.Business_ID equals bu.ID into bus
                            join emailVL in db.VanLangUsers on re.Email_VanLang equals emailVL.Email into emails
                            join intern in db.InternshipTopics on re.InternshipTopic_ID equals intern.ID into interns
                            where re.Business_ID == BusinessID && re.StatusRegistration == "Phê duyệt"
                            select new

                            {
                                id = re.ID,
                                fullname = re.VanLangUser.FullName,
                                email = re.Email_VanLang,
                                phone = re.VanLangUser.Mobile,
                                cv = re.CV,
                                status = re.StatusInternview,
                                semester = re.Semester.Semester1
                            };
            return Json(dataRegis, JsonRequestBehavior.AllowGet);
        }

        // GET: Business/InternViews/Details/5
        public ActionResult Details(int id)
        {
            var detailRegis = from re in db.Registrations
                              join post in db.Posts on re.Post_ID equals post.ID into posts
                              join sem in db.Semesters on re.Semester_ID equals sem.ID into sems
                              join bu in db.BusinessUsers on re.Business_ID equals bu.ID into bus
                              join emailVL in db.VanLangUsers on re.Email_VanLang equals emailVL.Email into emails
                              join intern in db.InternshipTopics on re.InternshipTopic_ID equals intern.ID into interns
                              where re.ID == id
                              select new
                              {
                                  id = re.ID,
                                  email = re.Email_VanLang,
                                  post_id = re.Post_ID,
                                  semester_id = re.Semester_ID,
                                  cv = re.CV,
                                  registrationdate = re.RegistrationDate,
                                  registrationModify = re.RegistrationModify,
                                  business_id = re.Business_ID,
                                  internviewresult = re.InterviewResult,
                                  internviewcomment = re.InterviewResultComment,
                                  statusinternview = re.StatusInternview,
                                  statusregistration = re.StatusRegistration,
                                  comment = re.Comment,
                                  interntopic_id = re.InternshipTopic_ID,
                                  internshipresult_id = re.InternshipResult_ID,
                                  major = re.VanLangUser.Major.Major1,
                                  buname = re.BusinessUser.BusinessName,
                                  student_id = re.VanLangUser.Student_ID,
                                  mobile = re.VanLangUser.Mobile,
                                  position = re.InternshipTopic.InternshipTopicName,
                                  bumail = re.BusinessUser.EmailContact,
                                  username = re.VanLangUser.FullName,
                              };
            return Json(detailRegis, JsonRequestBehavior.AllowGet);
        }

        // GET: Business/InternViews/Create
        public ActionResult Create()
        {
            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username");
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title");
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1");
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName");

            return View();
        }

        // POST: Business/InternViews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Email_VanLang,Post_ID,Semester_ID,CV,RegistrationDate,RegistrationModify,Business_ID,InterviewResult,InterviewResultComment,Status")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Registrations.Add(registration);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", registration.Business_ID);
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title", registration.Post_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", registration.Semester_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);

            return View(registration);
        }

        // GET: Business/InternViews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Registration registration = db.Registrations.Find(id);

            if (registration == null)
            {
                return HttpNotFound();
            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", registration.Business_ID);
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title", registration.Post_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", registration.Semester_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);

            return View(registration);
        }

        // POST: Business/InternViews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Email_VanLang,Post_ID,Semester_ID,CV,RegistrationDate,RegistrationModify,Business_ID,InterviewResult,InterviewResultComment,StatusInternview,StatusRegistration, InternshipTopic_ID, Comment, InternshipResult_ID")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                var email = db.VanLangUsers.Where(x => x.Email == registration.Email_VanLang).First();
                if (registration.StatusInternview == "Đậu Phỏng Vấn")
                {
                    var internship = db.InternshipResults.Where(x => x.ID == registration.InternshipResult_ID).FirstOrDefault();
                    /* if (db.InternshipResults.Any(x => x.Student_Email == registration.Email_VanLang && x.InternshipTopic_ID == registration.InternshipTopic_ID))
                     {
                         TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Cập Nhật Thành Công</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
                         return RedirectToAction("Index");

                     }*/
                    if (db.InternshipResults.Any(x => x.Student_Email == registration.Email_VanLang && x.Status == "Đang Thực Tập"))
                    {
                        TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">Sinh Viên Đang Thực Tập.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";

                        return RedirectToAction("Index");
                    }
                    string template = Server.MapPath("~/Areas/Admin/Views/Email/EmailBusinessInterview.cshtml");
                    string emailBody = System.IO.File.ReadAllText(template);
                    var fullname = db.Registrations.Where(x => x.VanLangUser.Email == registration.Email_VanLang).Select(x => x.VanLangUser.FullName).FirstOrDefault();

                    string To = registration.Email_VanLang;
                    var buName = db.Registrations.Where(x => x.Business_ID == registration.Business_ID).Select(x => x.BusinessUser.BusinessName).FirstOrDefault();
                    emailBody = emailBody.Replace("{studentName}", fullname);
                    emailBody = emailBody.Replace("{buName}", buName);
                    emailBody = emailBody.Replace("{StatusInternview}", registration.StatusInternview);
                    emailBody = emailBody.Replace("{InterviewResultComment}", registration.InterviewResultComment);
                    string Subject = "Thông Báo";
                    string Body = emailBody;
                    Outlook mail = new Outlook(To, Subject, Body);
                    mail.SendMail();
                    var InternShip = new InternshipResult();
                    InternShip.Student_Email = registration.Email_VanLang;
                    InternShip.MentorPoint = null;
                    InternShip.Mentor_Email = null;
                    InternShip.Semester_ID = null;
                    InternShip.MentorComment = null;
                    InternShip.Business_ID = registration.Business_ID;
                    InternShip.BusinessComment = null;
                    InternShip.BusinessPoint = null;
                    InternShip.Semester_ID = registration.Semester_ID;
                    InternShip.InternshipTopic_ID = registration.InternshipTopic_ID;
                    InternShip.Status = "Chờ Xác Nhận";
                    registration.InternshipResult_ID = InternShip.ID;
                    registration.InterviewResult = "Chờ Xác Nhận";

                    db.InternshipResults.Add(InternShip);
                    db.Entry(registration).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Cập Nhật Thành Công.</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
                    return RedirectToAction("Index");
                }
                else
                {
                    var getIDRe = db.InternshipResults.Where(x => x.ID == registration.InternshipResult_ID).Any();
                    if (getIDRe == false)
                    {
                        TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Cập Nhật Thành Công.</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
                        return RedirectToAction("Index");
                    }
                    if (db.InternshipResults.Any(x => x.ID == registration.InternshipResult_ID && x.Status == "Thực Tập Xong"))
                    {
                        TempData["AlertMessage"] = "<div class=\"toast toast--error\">\r\n     <div class=\"toast-left toast-left--error\">\r\n       <i class=\"fas fa-times-circle\"></i>\r\n     </div>\r\n     <div class=\"toast-content\">\r\n    <p class=\"toast-text\">Sinh Viên Đã Thực Tập Xong.</p>\r\n     </div>\r\n     <div class=\"toast-right\">\r\n       <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n     </div>\r\n   </div>";
                        return RedirectToAction("Index");
                    }
                    var asd = db.InternshipResults.Where(x => x.ID == registration.InternshipResult_ID).FirstOrDefault();
                    db.InternshipResults.Remove(asd);
                    registration.InterviewResult = "Chờ Xác Nhận";
                    registration.InternshipResult_ID = null;

                }
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "<div class=\"toast toast--success\">            <div class=\"toast-left toast-left--success\">               <i class=\"fas fa-check-circle\"></i>\r\n            </div>\r\n            <div class=\"toast-content\">\r\n                <p class=\"toast-text\">Cập Nhật Thành Công.</p>            </div>\r\n            <div class=\"toast-right\">\r\n                <i style=\"cursor:pointer\" class=\"toast-icon fas fa-times\" onclick=\"remove()\"></i>\r\n            </div>\r\n        </div>";
                return RedirectToAction("Index");

            }

            ViewBag.Business_ID = new SelectList(db.BusinessUsers, "ID", "Username", registration.Business_ID);
            ViewBag.Post_ID = new SelectList(db.Posts, "ID", "Title", registration.Post_ID);
            ViewBag.Semester_ID = new SelectList(db.Semesters, "ID", "Semester1", registration.Semester_ID);
            ViewBag.Email_VanLang = new SelectList(db.VanLangUsers, "Email", "FullName", registration.Email_VanLang);

            return View(registration);
        }
        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~/Uploads/CV/" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        // GET: Business/InternViews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Registration registration = db.Registrations.Find(id);

            if (registration == null)
            {
                return HttpNotFound();
            }

            return View(registration);
        }

        // POST: Business/InternViews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = db.Registrations.Find(id);

            db.Registrations.Remove(registration);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult ExportToExcel(int semester_id)
        {
            if (ModelState.IsValid)
            {
                int BusinessID = Convert.ToInt16(Session["BusinessID"]);

                var myTable = db.Registrations.Where(x => x.Business_ID == BusinessID && x.Semester_ID == semester_id).ToList();
                var intern = myTable.Select(x => new InternshipResult
                {
                    VanLangUser = x.VanLangUser,
                    Student_Email = x.VanLangUser.Email,
                    InternshipTopic_ID = x.InternshipTopic_ID,
                    InternshipTopic = x.InternshipTopic,


                }).ToList();

                if (db.Registrations.Where(x => x.Business_ID == BusinessID && x.Semester_ID == semester_id).Count() != 0)
                {
                    var getNameSemester = db.Registrations.Where(x => x.Business_ID == BusinessID && x.Semester_ID == semester_id).FirstOrDefault();
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
                        ws.Cells["A4:H4"].Merge = true;
                        ws.Cells["A1:C1"].Merge = true;
                        ws.Cells["A2:C2"].Merge = true;
                        ws.Cells["A3:C3"].Merge = true;
                        ws.Cells["A4"].Value = "Danh Sách Ứng Tuyển Học Kỳ " + getNameSemester.Semester.Semester1;
                        ws.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                        ws.Row(4).Style.Font.Bold = true;
                        ws.Row(4).Style.Font.Size = 18;



                        ws.Cells["A5"].Value = "STT";
                        ws.Cells["B5"].Value = "Họ Và Tên";
                        ws.Cells["C5"].Value = "MSSV";
                        ws.Cells["D5"].Value = "Số Điện Thoại";
                        ws.Cells["E5"].Value = "Email VLU";
                        ws.Cells["F5"].Value = "Vị Trí Thực Tập";
                        ws.Cells["G5"].Value = "Trạng Thái Phỏng Vấn";


                        int rowStart = 6;
                        int countSTT = 1;
                        foreach (var item in myTable)
                        {
                            if (item.StatusInternview == "Đậu Phỏng Vấn")
                            {
                                ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#b3ffb3"));
                                ws.Row(rowStart).Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Right.Style = ExcelBorderStyle.Thin;
                                ws.Row(rowStart).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            }
                            if (item.StatusInternview == "Rớt Phỏng Vấn")
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
                            ws.Cells[string.Format("E{0}", rowStart)].Value = item.VanLangUser.Email;
                            ws.Cells[string.Format("F{0}", rowStart)].Value = item.InternshipTopic.InternshipTopicName;
                            ws.Cells[string.Format("G{0}", rowStart)].Value = item.StatusInternview;
                            rowStart++;
                            countSTT++;
                        }
                        ws.Cells["A:AZ"].AutoFitColumns();
                        Response.Clear();
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=RegistrationReport.xlsx");
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
