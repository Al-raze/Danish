using System.Web.Mvc;
using BootstrapSupport;
using AkraTechFramework.Helpers;
using Infrastructure.Core.Models;
using System.Collections.Generic;
using AT.Core.Entities;
using AT.Core.Common;
using System.Configuration;

namespace AkraTechFramework.Controllers
{
    public class BaseController: Controller
    {
        protected GenericUserIdentity UserIdentity
        {
            get
            {
                return User.Identity.ToGenericUserIdentity();
            }
        }


        public bool IsAdmin
        {
            get
            {
                string userId = User.Identity.ToGenericUserIdentity().Name;

                return userId == ConfigurationManager.AppSettings["Environment.Admin"]; ;
            }
        }

        public void Attention(string message)
        {
            TempData.Add(Alerts.ATTENTION, message);
        }

        public void Success(string message)
        {
            TempData.Add(Alerts.SUCCESS, message);
        }

        public void Information(string message)
        {
            TempData.Add(Alerts.INFORMATION, message);
        }

        public void Error(string message)
        {
            TempData.Add(Alerts.ERROR, message);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                ViewBag.Menu = filterContext.HttpContext.Application.GetUserMenu(UserIdentity.AccessList);
            }
            else
                ViewBag.Menu = new List<MenuItem>();

            base.OnActionExecuting(filterContext);
        }
    }
}
