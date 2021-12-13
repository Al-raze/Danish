using Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using AT.Core.Common;
using AT.Core.Entities;

namespace AkraTechFramework.Helpers
{
    public static class AuthenticationHelper
    {
        public static string EncryptPassword(string password)
        {
            return EncryptionProvider.Encrypt(password, true,GlobalConstants.PASSWORD_KEY);
        }

        public static bool ValidatePassword(string passwordHash, string password)
        {
            try
            {
                if (EncryptionProvider.Decrypt(passwordHash, true,GlobalConstants.PASSWORD_KEY).Equals(password))
                    return true;
            }
            catch
            {
                return false;
            }

            return false; 
        }

        public static string DecryptPassowrd(string password)
        {
            try
            {
                return EncryptionProvider.Decrypt(password, true,GlobalConstants.PASSWORD_KEY);
            }
            catch
            {
            }

            return string.Empty;
        }

        public static GenericUserIdentity GetIdentity()
        {
            try
            {
                return HttpContext.Current.User.Identity.ToGenericUserIdentity();
            }
            catch
            {
            }

            return null;
        }

        public static bool UserHasRight(string rightName)
        {
            try
            {
                return HttpContext.Current.User.Identity.ToGenericUserIdentity().AccessRights.Contains(rightName);
            }
            catch
            {
            }

            return false;

        }


    }
}