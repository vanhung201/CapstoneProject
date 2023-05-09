using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using BusinessConnectManagement.Models;
using System.Data.Entity;
using BusinessConnectManagement.Middleware;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BusinessConnectManagement.Controllers
{
    public class AccountController : Controller
    {
        private BCMEntities db = new BCMEntities();
        private RoleRedirect redirect = new RoleRedirect();

        public async Task<ActionResult> Login()
        {
            if (Request.IsAuthenticated)
            {
                var user = await db.VanLangUsers.FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

                if (user.Status_ID == 2)
                {
                    SignOut();
                    return Redirect("~/quan-ly");
                }
                else
                {
                    user.Last_Access = DateTime.Now.ToString();
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return redirect.AutoRedirect(user.Role);
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

        public async Task<ActionResult> SignInCallback()
        {
            string email = User.Identity.Name;

            Session["EmailVLU"] = email;

            var user = await db.VanLangUsers.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                var newVanLangUser = new VanLangUser
                {
                    Email = email,
                    Last_Access = DateTime.Now.ToString(),
                    Status_ID = 1
                };

                if (email.Split('@').Last() == "vanlanguni.vn")
                {
                    newVanLangUser.Student_ID = email.Substring(email.IndexOf(".") + 1, email.LastIndexOf("@") - (email.IndexOf(".") + 1)).ToUpper();
                    var obj = (ClaimsIdentity)User.Identity;
                    string[] data = obj.Claims.Where(x => x.Type == "name").First().Value.Split('-');
                    newVanLangUser.FullName = data[1].Trim();
                    newVanLangUser.Student_ID = data[0].Trim();
                    newVanLangUser.Role = "Student";
                }

                if (email.Split('@').Last() == "vlu.edu.vn")
                {
                    var obj = (ClaimsIdentity)User.Identity;
                    string[] data = obj.Claims.Where(x => x.Type == "name").First().Value.Split('-');
                    newVanLangUser.FullName = data[0].Trim();
                    newVanLangUser.Role = "Mentor";
                }

                db.VanLangUsers.Add(newVanLangUser);
                await db.SaveChangesAsync();

                Session["Role"] = newVanLangUser.Role;

                return RedirectToAction("Information", "Account", new { area = "" });
            }
            else
            {
                if (user.Status_ID == 1)
                {
                    user.Last_Access = DateTime.Now.ToString();
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    Session["Role"] = user.Role;

                    return redirect.AutoRedirect(user.Role);
                }
                else
                {
                    Session["AccountBlocked"] = true;

                    return RedirectToAction("Login", "Account");
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
        [LoginVerification]
        [HttpGet]
        public ActionResult Information()
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
                if (user.Major_ID != null)
                {
                    user.Major_ID = vlu.Major_ID;

                }
                else
                {
                    TempData["message"] = "Vui Lòng Chọn Chuyên Ngành";
                    TempData["messageType"] = "warning";
                }
            }
            TempData["message"] = "Cập Nhật Thành Công";
            TempData["messageType"] = "success";
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Information", "Account");
        }
    }
}