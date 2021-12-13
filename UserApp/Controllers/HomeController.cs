using AkraTechFramework.Helpers;
using Infrastructure.Core;
using Infrastructure.Core.DataAccess;
using Infrastructure.Core.Models;
using Infrastructure.Core.Providers;
using AT.Core.Common;
using AT.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace AkraTechFramework.Controllers
{
    public class HomeController : BaseController
    {
        private IMembershipProvider _memberShipProvider;
        readonly log4net.ILog _log;

        public HomeController(IMembershipProvider membershipProvider)
        {
            _log = log4net.LogManager.GetLogger("log4net-FileAppender");
            this._memberShipProvider = membershipProvider;
        }

        public HomeController()
            : this(new CustomMembershipProvider(new UserRepository()))
        {

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn(string returnUrl)
        {
            Error("Your Session has expired,please sign in to continue");
            return RedirectToAction("Index");
        }


        public ActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Tender");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SetPageLanguage(string pageLanguage)
        {
            HttpCookie languageCookie = Request.Cookies["PageLanguage"];

            if (languageCookie == null)
            {
                languageCookie = new HttpCookie("PageLanguage");
                languageCookie.Expires = DateTime.Now.AddDays(360);
            }

            languageCookie.Values["PageLanguage"] = pageLanguage;

            Response.Cookies.Remove("PageLanguage");

            Response.Cookies.Add(languageCookie);

            return Content("Language Cookie Set");
        }


        public ActionResult ClearLanguageCache()
        {
            new LanguageProvider().ClearLanguageCache();

            return Content("Cache Cleared Successfully");
        }

        public ActionResult Login(string user, string password, string returnUrl)
        {
            UserInformation userInfo = _memberShipProvider.FindUserByName(user);

            if (userInfo != null && AuthenticationHelper.ValidatePassword(userInfo.Password, password))
            {
                var accessibleMenuItems = _memberShipProvider.GetAccessibleMenuItems(userInfo.RoleID);

                List<int> accessList = accessibleMenuItems.Select((m) => m.MenuID).ToList();
                accessList.Sort();
                List<string> rightsList = accessibleMenuItems.Select((m) => m.Name).ToList();

                var serializer = new JavaScriptSerializer();

                GenericUser genericUser = new GenericUser() { UserName = userInfo.UserName, UserID = userInfo.UserID, RoleID = userInfo.RoleID, MenuAccessList = accessList, RoleName = userInfo.RoleName, Language = "AR", MenuAccessRights = rightsList };
                string userData = serializer.Serialize(genericUser);

                DateTime expire = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(userInfo.UserID, userInfo.UserName, DateTime.Now, expire, false, userData);
                string hashTicket = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
                HttpContext.Response.Cookies.Add(cookie);

                return RedirectToAction("Dashboard", "Tender");
            }


            Error("Invalid Username or Password");

            return View("Index");
        }


        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult TenderSearch(string tenderNO)
        {
            ViewBag.TenderNO = tenderNO;
            return View();
        }

        [HttpPost]
        public JsonResult GetTenderSearch(String TenderNo)
        {
            try
            {
                List<Tender> tender = _memberShipProvider.GetTenderSearchDetails(TenderNo);
                return Json(new { Result = "OK", Records = tender });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
