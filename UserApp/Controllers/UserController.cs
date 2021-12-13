using AkraTechFramework.Controllers;
using AkraTechFramework.Helpers;
using Infrastructure.Core.Models;
using Infrastructure.Core.Provider;
using Infrastrucutre.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManager.Controllers
{
     [Authorize]
    public class UserController :  BaseController
    {
        //
        // GET: /User/

        SettingsProvider _settingProvider;

        public UserController()
        {
            _settingProvider = new SettingsProvider(new SettingsRepository());            
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult UpdatePassword(ChangePassword password)
        {
            if (password.ConfirmPassword != password.NewPassword)
            {
                Attention("Passwords do not match");
                return View("ChangePassword", password);
            }

            var identity = AuthenticationHelper.GetIdentity();

            var generated = new ChangePassword() { UserID = identity.UserID, NewPassword = AuthenticationHelper.EncryptPassword(password.NewPassword) };

            bool success = _settingProvider.ChangePassword(generated);

            if (success)
            {
                Success("Password Udpated Successfully.");
                return RedirectToAction("Dashboard","Tender");
            }
            else
            {
                Attention("Please verify the fields entered");
                return View("ChangePassword", password);
            }
        }

    }
}
