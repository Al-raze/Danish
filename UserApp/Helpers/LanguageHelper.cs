using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Core.Providers;
using Infrastructure.Constants;

namespace AkraTechFramework.Helpers
{
    public static class LanguageHelper
    {
        public static string CurrentLanguage
        {
            get
            {
               //**********For this web app the language is always english
 
                string pageLanguage = GlobalConstants.USER_LANG_ENGLISH;

                //try
                //{
                //    if (HttpContext.Current.Request.Cookies["PageLanguage"] != null)
                //    {
                //        pageLanguage = HttpContext.Current.Request.Cookies["PageLanguage"].Values[0];
                //    }
                //}
                //catch
                //{

                //}

                return pageLanguage;
            }
        }


        public static string GetLabelCaption(string labelName, string pageName)
        {
            return LanguageProvider.GetLabelCaption(CurrentLanguage, labelName, pageName);
        }

    }
}