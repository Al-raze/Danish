using Infrastructure.Constants;
using Infrastructure.Core.DataAccess;
using Infrastructure.Core.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AkraTechFramework
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private IUserRepository _userRepository;
        private log4net.ILog _log;

        public MvcApplication(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
        public MvcApplication() : this(new UserRepository())
        {
            _log = log4net.LogManager.GetLogger("log4net-FileAppender");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            Application.Set(GlobalConstants.USER_MENUS,_userRepository.GetMenuItems());
             
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            if (exception != null)
            {
                _log.Error("UnExpected Error from user " + User.Identity.Name , exception);
            }
         
        }

    }
}