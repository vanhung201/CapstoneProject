using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;
using System.Data.Entity;
using System.Web.Helpers;
using BusinessConnectManagement.Areas.Admin.Middleware;

namespace BusinessConnectManagement.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private BCMEntities db = new BCMEntities();
        private CheckUserRole checkUserRole = new CheckUserRole();

        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                string email = User.Identity.Name;
                var query = db.VanLangUsers.FirstOrDefault(x => x.Email == email);

                return checkUserRole.RedirectToPage(query.Role_ID.Value);
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
            string email = User.Identity.Name;
            var query = db.VanLangUsers.FirstOrDefault(x => x.Email == email);

            if (query == null)
            {
                VanLangUser newVanLangUser = new VanLangUser();

                newVanLangUser.Email = email;
                newVanLangUser.Role_ID = 4;
                newVanLangUser.Last_Access = DateTime.Now;
                newVanLangUser.Status_ID = 1;

                db.VanLangUsers.Add(newVanLangUser);
                db.SaveChanges();

                Session["fullname"] = email;

                return RedirectToAction("Index", "Home", new { area = "Student" });
            }
            else
            {
                var currentVanLangUser = db.VanLangUsers.Where(x => x.Email == email).FirstOrDefault();

                currentVanLangUser.Last_Access = DateTime.Now;

                db.Entry(currentVanLangUser).State = EntityState.Modified;
                db.SaveChanges();

                Session["fullname"] = currentVanLangUser.FullName;

                return checkUserRole.RedirectToPage(currentVanLangUser.Role_ID.Value);
            }
        }

        public void SignOut()
        {
            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
        }

        public ActionResult SignOutCallback()
        {
            if (Request.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login", "Account");
        }
    }
}