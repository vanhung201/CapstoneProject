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
                    TempData["message"] = "Tài khoản của bạn đang bị khóa. Vui lòng liên hệ Khoa CNTT để được hỗ trợ.";
                    TempData["messageType"] = "warning";

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
                        TempData["message"] = "Đăng Nhập Thất Bại. Sai Tài Khoản Hoặc Mật Khẩu";
                        TempData["messageType"] = "error";


                        return View();
                    }
                }
            }
            else
            {
                TempData["message"] = "Tài Khoản Không Tồn Tại";
                TempData["messageType"] = "warning";

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