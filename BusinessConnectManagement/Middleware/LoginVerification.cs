using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BusinessConnectManagement.Middleware
{
    public class BusinessVerification : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["BusinessID"] == null)
            {
                filterContext.Result = new RedirectResult("~/doanh-nghiep/dang-nhap");
                return;
            }
        }
    }

    public class LoginVerification : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["EmailVLU"] == null)
            {
                filterContext.Result = new RedirectResult("~/quan-ly");
                return;
            }
        }
    }

    public class AdminVerification : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["EmailVLU"] == null || filterContext.HttpContext.Session["Role"].ToString() != "Admin")
            {
                filterContext.Result = new RedirectResult("~/quan-ly");
                return;
            }
        }
    }

    public class MentorVerification : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["EmailVLU"] == null || filterContext.HttpContext.Session["Role"].ToString() != "Mentor")
            {
                filterContext.Result = new RedirectResult("~/quan-ly");
                return;
            }
        }
    }
}