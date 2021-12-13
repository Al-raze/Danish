using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Constants
{
    public static class SessionKeyConstants
    {
        public const string SESSION_USER_MENU = "ROLE_MENUITEMS";        
    }

    public static class MessageConstants
    {
        public const string USER_ALREADY_REGISTERED = "You have already registered for Web access please login with your UserName and Password";
        public const string USER_REGISTRATION_FAILED = "Registration Failed , Please verify if your details are correct or contact ###-### for assistance";
        public const string USER_ALREADY_EXISTS = "Another user has already selected the username.Please select another username";
        public const string MAILID_NOT_REGISTERED = "The e-mail address you have entered is incorrect. Please enter registered e-mail address";
        public const string EMAIL_ALREADY_EXISTS = "The entered e-mail address is already registered. Please enter a new e-mail address";
        public const string OLD_PASSWORD_INCORRECT = "The old password you have entered is invalid";
        public const string UNEXPECTED_ERROR = "You have encountered an unexpected error.";
        public const string COMPANY_UNEXPECTED_ERROR = "You have encountered an unexpected error.";
        public const string TENDER_ERROR = "Please Verify the entered details.";
        public const string MESSAGE_SUCCESS = "SUCCEED";
        
        
        
        
    }

    public static class GlobalConstants
    {
        public const string USER_MENUS = "USER_MENUS";
        public const string USER_LANG_ARABIC = "Arabic";
        public const string USER_LANG_ENGLISH = "English";
        public const string COMPANY_NAME = "Company.Name";
        public const string COMPANY_LOGO = "Company.Logo";
        public const string PASSWORD_KEY = "tendermanager";
        public const string CHANNEL_AMAZON = "AMAZON";
        public const string CHANNEL_EBAY = "EBAY";

    }


    public static class ErrorCodes
    {
        public const string USER_ALREADY_EXISTS = "101";
    }
}
