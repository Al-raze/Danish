using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkraTechFramework.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            Exception exception = Server.GetLastError();
            return View();
        }

    }
}
