using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Business.Controllers
{
    public class AuthController : Controller
    {
        private BCMEntities db = new BCMEntities();

        // GET: Business/Login
        public ActionResult Login()
        {
            Session["username-incorrect"] = false;
            Session["password-incorrect"] = false;

            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var query = db.BusinessUsers.FirstOrDefault(x => x.Username.Equals(username));

            if (query != null)
            {
                if (query.Status_ID == 2)
                {
                    Session["BusinessAccountBlocked"] = true;

                    return View();
                }
                else
                {
                    if (query.Password.Equals(password))
                    {
                        Session["BusinessID"] = query.ID;
                        Session["BusinessName"] = query.BusinessName;

                        query.Last_Access = (DateTime.Now).ToString();

                        db.Entry(query).State = EntityState.Modified;
                        db.SaveChanges();

                        return RedirectToAction("Index", "BusinessHome");
                    }
                    else
                    {
                        Session["password-incorrect"] = true;

                        return View();
                    }
                }
            }
            else
            {
                Session["username-incorrect"] = true;

                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["BusinessID"] = null;
            Session["BusinessName"] = null;

            return RedirectToAction("Login", "Auth");
        }
    }
}