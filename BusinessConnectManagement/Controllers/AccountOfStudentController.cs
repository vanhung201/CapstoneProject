using BusinessConnectManagement.Models;
using BusinessConnectManagement.Middleware;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Security.Claims;

namespace BusinessConnectManagement.Controllers
{
    public class AccountOfStudentController : Controller
    {
        private BCMEntities db = new BCMEntities();

        public ActionResult Login()
        {
            var query = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (Request.IsAuthenticated)
            {
                if (query.Status_ID == 2)
                {
                    Response.Redirect("~/trang-dang-nhap");
                }
                else
                {
                    query.Last_Access = DateTime.Now.ToString();

                    db.Entry(query).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            return View();
        }

        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = Url.Action("SignInCallback") },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public ActionResult SignInCallback()
        {
            string email = User.Identity.Name;

            Session["EmailVLU"] = email;

            var query = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();

            if (query == null)
            {
                VanLangUser newVanLangUser = new VanLangUser();

                newVanLangUser.Email = email;
                newVanLangUser.Role = "Student";
                newVanLangUser.Last_Access = DateTime.Now.ToString();
                newVanLangUser.Status_ID = 1;

                if (email.Split('@').Last() == "vanlanguni.vn")
                {
                    //newVanLangUser.Student_ID = email.Substring(email.IndexOf(".") + 1, email.LastIndexOf("@") - (email.IndexOf(".") + 1)).ToUpper();
                    var obj = (ClaimsIdentity)User.Identity;
                    string[] data = obj.Claims.Where(x => x.Type == "name").First().Value.Split('-');
                    newVanLangUser.FullName = data[1].Trim();
                    newVanLangUser.Student_ID = data[0].Trim();
                }

                db.VanLangUsers.Add(newVanLangUser);
                db.SaveChanges();

                return RedirectToAction("StudentInformation", "AccountOfStudent", new { area = "" });
            }
            else
            {
                var currentVanLangUser = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();

                if (currentVanLangUser.Status_ID == 1)
                {
                    currentVanLangUser.Last_Access = DateTime.Now.ToString();

                    db.Entry(currentVanLangUser).State = EntityState.Modified;
                    db.SaveChanges();

                    Session["FullNameOfStudent"] = currentVanLangUser.FullName;

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    Session["AccountBlocked"] = true;

                    return RedirectToAction("Login", "AccountOfStudent", new { area = "" });
                }
            }
        }

        // Send an OpenID Connect sign-out request.
        public void SignOut()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            }

            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);

            Response.Redirect("~/trang-dang-nhap");
        }

        public ActionResult StudentInformation()
        {
            var currentVanLangUser = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (String.IsNullOrEmpty(currentVanLangUser.Student_ID) || String.IsNullOrEmpty(currentVanLangUser.FullName) || String.IsNullOrEmpty(currentVanLangUser.Mobile) || currentVanLangUser.Major_ID <= 0 || currentVanLangUser.Major_ID == null)
            {
                ViewBag.MajorList = db.Majors.ToList();

                return View(currentVanLangUser);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult UpdateStudentInformation(VanLangUser user)
        {
            var query = db.VanLangUsers.Where(x => x.Email == user.Email).FirstOrDefault();

            if (query != null)
            {
                query.Student_ID = user.Student_ID;
                query.FullName = user.FullName;
                query.Mobile = user.Mobile;
                if (user.Major_ID != null)
                {
                    query.Major_ID = user.Major_ID;

                }
                else
                {
                    TempData["message"] = "Vui Lòng Chọn Chuyên Ngành";
                    TempData["messageType"] = "warning";
                }

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();
            }

            Session["FullNameOfStudent"] = query.FullName;

            return RedirectToAction("Index", "Home", new { area = "" });
        }
        [LoginVerification]
        [HttpGet]
        public ActionResult InformationStudent()
        {
            var email = User.Identity.Name;
            var curUser = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();
            ViewBag.major = db.Majors.ToList();
            return View(curUser);
        }
        [LoginVerification]
        [HttpPost]
        public ActionResult UpdateInfo(VanLangUser vlu)
        {
            var user = db.VanLangUsers.Where(x => x.Email == vlu.Email).FirstOrDefault();

            if (user != null)
            {
                user.Student_ID = vlu.Student_ID;
                user.FullName = vlu.FullName;
                user.Major_ID = vlu.Major_ID;
                user.Mobile = vlu.Mobile;
            }
            TempData["message"] = "Cập Nhật Thành Công";
            TempData["messageType"] = "success";
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("InformationStudent", "AccountOfStudent");
        }


    }
}