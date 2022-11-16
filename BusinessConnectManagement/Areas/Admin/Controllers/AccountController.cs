using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessConnectManagement.Models;
using System.Data.Entity;
using System.Web.Helpers;

namespace BusinessConnectManagement.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private BCMEntities db = new BCMEntities();

        public ActionResult Login()
        {
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

            var query = db.VanLangUsers.FirstOrDefault(x => x.Email == User.Identity.Name);

            if (query == null)
            {
                VanLangUser newVanLangUser = new VanLangUser();

                newVanLangUser.Email = User.Identity.Name;
                newVanLangUser.Role_ID = 4;
                newVanLangUser.Last_Access = DateTime.Now;
                newVanLangUser.Status = true;

                db.VanLangUsers.Add(newVanLangUser);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var currentVanLangUser = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

                currentVanLangUser.Last_Access = DateTime.Now;

                db.Entry(currentVanLangUser).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
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