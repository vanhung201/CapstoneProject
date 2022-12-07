using BusinessConnectManagement.Middleware;
using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Areas.Faculty.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private BCMEntities db = new BCMEntities();
        private RoleRedirect roleRedirect = new RoleRedirect();

        // GET: Faculty/Home
        public ActionResult Index()
        {
            var query = db.VanLangUsers.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            if (query.Role_ID == 2)
            {
                // Update Last Access when user login without click Login button
                query.Last_Access = DateTime.Now;

                db.Entry(query).State = EntityState.Modified;
                db.SaveChanges();

                return View();
            }
            else
            {
                return roleRedirect.Redirect(query.Role_ID.Value);
            }
        }
    }
}