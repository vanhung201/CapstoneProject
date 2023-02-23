﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Controllers
{
    [LoginVerification]
    public class RegistrationController : Controller
    {
        private BCMEntities db = new BCMEntities();
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Apply(Registration registration, HttpPostedFileBase CV)
        {
            var post = db.Posts.Where(x => x.ID == registration.Post_ID).FirstOrDefault();
            var email = User.Identity.Name;
            var isExist = db.Registrations.Any(x => x.Email_VanLang == email && x.Post_ID == post.ID);
            if (isExist)
            {
                TempData["message"] = "Bạn đã ứng tuyển cho bài viết này rồi";
                TempData["messageType"] = "warning";
                return RedirectToAction("Details", "Posts", new { id = post.ID });

            }
            else
            {
                if(CV != null)
                {
                    if(Path.GetExtension(CV.FileName).ToLower() != ".pdf")
                    {
                        TempData["message"] = "Vui lòng chọn file pdf";
                        TempData["messageType"] = "error";
                        return RedirectToAction("Details", "Posts", new { id = post.ID });
                    } else
                    {
                        using (var scope = new TransactionScope())
                        {
                            registration.CV = CV.FileName;
                            var path = Server.MapPath("~/Uploads/CV/");
                            CV.SaveAs(path + registration.CV);
                            registration.Email_VanLang = email;
                            registration.Semester_ID = 1;
                            registration.RegistrationDate = (DateTime.Now).ToString();
                            registration.RegistrationModify = (DateTime.Now).ToString();
                            registration.InterviewResult = null;
                            registration.InterviewResultComment = null;
                            registration.StatusInternview = null;
                            registration.StatusRegistration = "Chờ Phê Duyệt";
                            registration.Comment = null;
                            db.Registrations.Add(registration);
                            db.SaveChanges();
                            scope.Complete();
                            TempData["message"] = "Ứng tuyển thành công";
                            TempData["messageType"] = "success";
                            return RedirectToAction("Details", "Posts", new { id = post.ID });
                        }
                    }
                    
                }
                else
                {
                    TempData["message"] = "Thiếu Thông Tin Ứng Tuyển";
                    TempData["messageType"] = "error";
                    return RedirectToAction("Details", "Posts", new { id = post.ID });
                }
                
            }
        }

        public ActionResult Remove(int id) 
        {
            Registration registration = db.Registrations.Find(id);
            var regStatus = registration.StatusRegistration;
            var post = db.Posts.Where(x => x.ID == registration.Post_ID).FirstOrDefault();
            if(regStatus == "Phê Duyệt")
            {
                TempData["message"] = "Hủy Thất Bại Đơn Của Bạn Đã Được Duyệt";
                TempData["messageType"] = "error";
                return RedirectToAction("Details", "Posts", new { id = post.ID });

            } else
            {
            db.Registrations.Remove(registration);
            db.SaveChanges();
            TempData["message"] = "Hủy Kết Quả Thành Công";
            TempData["messageType"] = "success";
            return RedirectToAction("Details", "Posts", new { id = post.ID });
            }
        }

        public ActionResult getRegistrationData(int id)
        {
            var data = (from reg in db.Registrations
                        where reg.ID == id
                        select new
                        {
                           ID= reg.ID,
                           CV = reg.CV,
                           Position = reg.Position
                        });

            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int id, HttpPostedFileBase CV)
        {
            Registration registration = db.Registrations.Find(id);
            var regStatus = registration.StatusRegistration;
            var post = db.Posts.Where(x => x.ID == registration.Post_ID).FirstOrDefault();
            
            if(regStatus == "Phê Duyệt")
            {
                TempData["message"] = "Cập Nhật Thất Bại Đơn Của Bạn Đã Được Duyệt";
                TempData["messageType"] = "error";
                return RedirectToAction("Details", "Posts", new { id = post.ID });
            }
            else
            {
                if (CV != null)
                {
                    if (Path.GetExtension(CV.FileName).ToLower() != ".pdf")
                    {
                        TempData["message"] = "Vui lòng chọn file pdf";
                        TempData["messageType"] = "error";
                        return RedirectToAction("Details", "Posts", new { id = post.ID });
                    }
                    else
                    {
                        using (var scope = new TransactionScope())
                        {
                            registration.CV = CV.FileName;
                            var path = Server.MapPath("~/Uploads/CV/");
                            CV.SaveAs(path + registration.CV);
                            db.Entry(registration).State = EntityState.Modified;
                            db.SaveChanges();
                            scope.Complete();
                            TempData["message"] = "Cập nhật thành công";
                            TempData["messageType"] = "success";
                            return RedirectToAction("Details", "Posts", new { id = post.ID });
                        }
                    }

                }
                else
                {
                    TempData["message"] = "Thiếu Thông Tin Ứng Tuyển";
                    TempData["messageType"] = "error";
                    return RedirectToAction("Details", "Posts", new { id = post.ID });
                }
            }
            
        }

        public ActionResult getRegList()
        {
            var email = User.Identity.Name;

            var data = (from reg in db.Registrations
                        where reg.Email_VanLang == email
                        select new
                        {
                            ID = reg.ID,
                            BusinessName = reg.BusinessUser.BusinessName,
                            Position = reg.Position,
                            Comment = reg.Comment,
                            Status = reg.StatusRegistration,
                            Post_ID = reg.Post_ID
                        });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult listReg()
        {
            return View();
        }
    }
}