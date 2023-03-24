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
                    HttpContext.GetOwinContext().Authentication.SignOut(
                        OpenIdConnectAuthenticationDefaults.AuthenticationType,
                        CookieAuthenticationDefaults.AuthenticationType);

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
                    Role = "Student",
                    Last_Access = DateTime.Now.ToString(),
                    Status_ID = 1
                };

                if (email.Split('@').Last() == "vanlanguni.vn")
                {
                    newVanLangUser.Student_ID = email.Substring(email.IndexOf(".") + 1, email.LastIndexOf("@") - (email.IndexOf(".") + 1)).ToUpper();
                }

                db.VanLangUsers.Add(newVanLangUser);
                await db.SaveChangesAsync();

                Session["Role"] = "Student";

                return RedirectToAction("Index", "Home", new { area = "" });
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

            Response.Redirect("~/quan-ly");
        }
    }
}