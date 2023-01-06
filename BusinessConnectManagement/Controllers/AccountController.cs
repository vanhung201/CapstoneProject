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

namespace BusinessConnectManagement.Controllers
{
    public class AccountController : Controller
    {
        private BCMEntities db = new BCMEntities();
        private RoleRedirect redirect = new RoleRedirect();

        public ActionResult Login()
        {
            var query = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (Request.IsAuthenticated)
            {
                if (query.Status_ID == 2)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);

                    Response.Redirect("~/quan-ly");
                }
                else
                {
                    query.Last_Access = DateTime.Now;

                    db.Entry(query).State = EntityState.Modified;
                    db.SaveChanges();

                    return redirect.AutoRedirect(query.Role);
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
                newVanLangUser.Last_Access = DateTime.Now;
                newVanLangUser.Status_ID = 1;

                if (email.Split('@').Last() == "vanlanguni.vn")
                {
                    newVanLangUser.Student_ID = email.Substring(email.IndexOf(".") + 1, email.LastIndexOf("@") - (email.IndexOf(".") + 1)).ToUpper();
                }

                db.VanLangUsers.Add(newVanLangUser);
                db.SaveChanges();

                Session["Role"] = "Student";

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                var currentVanLangUser = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();

                if (currentVanLangUser.Status_ID == 1)
                {
                    currentVanLangUser.Last_Access = DateTime.Now;

                    db.Entry(currentVanLangUser).State = EntityState.Modified;
                    db.SaveChanges();

                    Session["Role"] = currentVanLangUser.Role;

                    return redirect.AutoRedirect(currentVanLangUser.Role);
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
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);

            Response.Redirect("~/quan-ly");
        }
    }
}
