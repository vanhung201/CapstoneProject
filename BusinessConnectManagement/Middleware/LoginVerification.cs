using BusinessConnectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                filterContext.Result = new RedirectResult("~/Business/Auth/Login");
                return;
            }
        }
    }
}