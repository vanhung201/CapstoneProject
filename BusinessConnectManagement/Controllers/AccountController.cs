/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System.Data.Entity;

namespace BusinessConnectManagement.Controllers
{
    public class AccountController : Controller
    {
        private BCMEntities db = new BCMEntities();
        private RoleRedirect roleRedirect = new RoleRedirect();

        public ActionResult Login()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            if (Request.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return View();
        }

        public void SignIn()
        {
            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = Url.Action("SignInCallback") },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public ActionResult SignInCallback()
        {
            var query = db.VanLangUsers.FirstOrDefault(x => x.Email == User.Identity.Name);

            if (query == null)
            {
                VanLangUser newVanLangUser = new VanLangUser();

                newVanLangUser.Email = User.Identity.Name;
                newVanLangUser.Role_ID = 4;
                newVanLangUser.Last_Access = DateTime.Now;
                newVanLangUser.Status_ID = 1;

                db.VanLangUsers.Add(newVanLangUser);
                db.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "Student" });
            }
            else
            {
                var currentVanLangUser = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                currentVanLangUser.Last_Access = DateTime.Now;

                db.Entry(currentVanLangUser).State = EntityState.Modified;
                db.SaveChanges();

                return roleRedirect.Redirect(currentVanLangUser.Role_ID.Value);
            }
        }

        // Send an OpenID Connect sign-out request.
        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);

            Response.Redirect("~/Account/Login");
        }
    }
}
*/