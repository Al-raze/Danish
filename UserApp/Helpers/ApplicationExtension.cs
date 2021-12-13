using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infrastructure.Core.Models;
using Infrastructure.Constants;
using AT.Core.Entities;

namespace AkraTechFramework.Helpers
{
    public static class ApplicationExtension
    {
        public static List<MenuItem> GetUserMenu(this HttpApplicationStateBase applicationState,List<int> accessList)
        {
            var userMenu = applicationState.Get(GlobalConstants.USER_MENUS);
            
            if(userMenu !=null)
            {
                var filteredList = (userMenu as List<MenuItem>).Where(m => accessList.Contains(m.MenuID)).ToList();

                return filteredList;
            }
            else
	        {
                return new List<MenuItem>();
	        }
        }

        public static List<MenuItem> GetMenuList(this HttpApplicationStateBase applicationState, List<int> accessList)
        {
            var userMenu = applicationState.Get(GlobalConstants.USER_MENUS);

            try
            {
                return userMenu as List<MenuItem>;
            }
            catch (Exception)
            {
                
            }

            return new List<MenuItem>();
            
        }
    }
}