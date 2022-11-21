using System.Linq;
using System.Web.Mvc;
using BusinessConnectManagement.Areas.Admin.Middleware;
using BusinessConnectManagement.Models;

namespace BusinessConnectManagement.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private BCMEntities db = new BCMEntities();
        private CheckUserRole checkUserRole = new CheckUserRole();

        // GET: Admin/Home
        public ActionResult Index()
        {
            string email = User.Identity.Name;
            var query = db.VanLangUsers.FirstOrDefault(x => x.Email == email);

            if (query.Role_ID.Value == 1)
            {
                var fullName = Session["fullname"];
                GetFullNameWhenLoggedIn getFullNameWhenLoggedIn = new GetFullNameWhenLoggedIn();
                Session["fullname"] = getFullNameWhenLoggedIn.IsLoggedIn(fullName, email);
            }
            else
            {
                return checkUserRole.RedirectToPage(query.Role_ID.Value);
            }

            return View();
        }
    }
}