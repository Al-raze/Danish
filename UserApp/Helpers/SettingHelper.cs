using Infrastructure.Constants;
using Infrastructure.Core.Models;
using Infrastructure.Core.Models.ViewModels;
using Infrastructure.Core.Provider;
using Infrastrucutre.Core.DataAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkraTechFramework.Helpers
{
    public static class SettingHelper
    {
        static WebSetting _setting; 

        public static WebSetting GetSetting()
        {
            if (_setting == null)
            {
                _setting = new SettingsProvider(new SettingsRepository()).GetWebSetting();
            }

            return _setting;
        }
      

        public static void ResetWebSetting()
        {
            _setting = null;
        }

    }
}