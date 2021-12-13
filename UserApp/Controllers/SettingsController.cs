using AkraTechFramework.Helpers;
using Infrastructure.Constants;
using Infrastructure.Core.Models;
using Infrastructure.Core.Provider;
using Infrastrucutre.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AkraTechFramework.Controllers
{
    [Authorize]
    public class SettingsController : BaseController
    {
        //
        // GET: /Settings/

        private ISettingsProvider _settingsProvider;
        IListingProvider _listingProvider;

        public SettingsController(ISettingsProvider settingsProvider, IListingProvider listingProvider)
        {
            this._settingsProvider = settingsProvider;
            this._listingProvider = listingProvider;
        }

        public SettingsController()
            : this(new SettingsProvider(new SettingsRepository()), new ListingProvider(new ListingRepository()))
        {

        }
        //-----------------------------------------Group Assigned 08-02-2021--------------------------------------//
        public ActionResult GroupAssign() // Group Controller for Assigned Item to the User 03-02-2021 
        {
            loadUsersList();
            loadItemList();
            return View();
        }
        

        private void loadUsersList() //User List
        {
            var User = _listingProvider.GetUser(sync: true);
            ViewBag.User = new SelectList(User, "UserID", "UserName");

        }

        private void loadItemList() //Items List
        {
            var Item = _listingProvider.GetItemsList();
            ViewBag.Item = new SelectList(Item, "ItemMasterID", "ItemName");

        }

        public JsonResult AddGroup(AddGroup user)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Json(user);
            //}


            bool IsAddedSuccess = _settingsProvider.AddGroup(user);

            if (IsAddedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }


        //__________________________________________________________________________________//
        [HttpPost]
        public JsonResult GetGroupDetails(int jtStartIndex = 0, int jtPageSize = 0)
        {
            int rowCount = 0;
            var users = _settingsProvider.GetGroupDetails(out rowCount, jtStartIndex, jtPageSize);
            return Json(new { Result = "OK", Records = users, TotalRecordCount = rowCount, CurrentPage = jtStartIndex });
        }

        //__________________________________________________________________________________//
        public ActionResult DeleteGroup(int id)
        {
            bool IsDeletedSuccessfully = _settingsProvider.DeleteGroup(id);

            if (IsDeletedSuccessfully)
                Success("Group Deleted Successfully");
            else
                Error(MessageConstants.UNEXPECTED_ERROR);

            return RedirectToAction("GroupAssign");
        }
        //--------------------------------------------------------------------------------------------------------//
        public ActionResult CompanyMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCompanyDetails()
        {
            try
            {
                List<Company> company = _settingsProvider.GetCompanyMaster();
                return Json(new { Result = "OK", Records = company });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult AddCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return Json(company);
            }

            bool IsAddedSuccess = _settingsProvider.AddCompanyMaster(company);

            if (IsAddedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        public JsonResult GetCompanyByID(int id)
        {
            Company user = _settingsProvider.GetCompanyMasterByID(id);
            return Json(user);
        }

        [HttpPost]
        public JsonResult UpdateCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return Json(company);
            }

            bool IsUpdatedSuccess = _settingsProvider.UpdateCompanyMaster(company);

            if (IsUpdatedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        //public ActionResult DeleteUser(int id)
        //{
        //    bool IsDeletedSuccessfully = _settingsProvider.DeleteUser(id);

        //    if (IsDeletedSuccessfully)
        //        Success("User Deleted Successfully");
        //    else
        //        Error(MessageConstants.UNEXPECTED_ERROR);

        //    return RedirectToAction("ManageUsers");
        //}



        public ActionResult BankMaster()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetBankMaster()
        {
            try
            {
                List<Bank> bank = _settingsProvider.GetBankMaster();
                return Json(new { Result = "OK", Records = bank });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult AddBankDetails(Bank bank)
        {
            if (!ModelState.IsValid)
            {
                return Json(bank);
            }

            bool IsAddedSuccess = _settingsProvider.AddBankMaster(bank);

            if (IsAddedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        public JsonResult GetBankByID(int id)
        {
            Bank bank = _settingsProvider.GetBankMasterByID(id);
            return Json(bank);
        }

        [HttpPost]
        public JsonResult UpdateBank(Bank bank)
        {
            if (!ModelState.IsValid)
            {
                return Json(bank);
            }

            bool IsUpdatedSuccess = _settingsProvider.UpdateBankMaster(bank);

            if (IsUpdatedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        //public ActionResult DeleteUser(int id)
        //{
        //    bool IsDeletedSuccessfully = _settingsProvider.DeleteUser(id);

        //    if (IsDeletedSuccessfully)
        //        Success("User Deleted Successfully");
        //    else
        //        Error(MessageConstants.UNEXPECTED_ERROR);

        //    return RedirectToAction("ManageUsers");
        //}



        public ActionResult RivalMaster()
        {
            return View();
        }

        public ActionResult SwitchEnglish()
        {
            HttpCookie languageCookie = Request.Cookies["PageLanguage"];

            if (languageCookie == null)
            {
                languageCookie = new HttpCookie("PageLanguage");
                languageCookie.Expires = DateTime.Now.AddDays(360);
            }

            languageCookie.Values["PageLanguage"] = GlobalConstants.USER_LANG_ENGLISH;

            Response.Cookies.Remove("PageLanguage");

            Response.Cookies.Add(languageCookie);

            return RedirectToAction("Dashboard", "Tender");
        }

        public ActionResult SwitchArabic()
        {
            HttpCookie languageCookie = Request.Cookies["PageLanguage"];

            if (languageCookie == null)
            {
                languageCookie = new HttpCookie("PageLanguage");
                languageCookie.Expires = DateTime.Now.AddDays(360);
            }

            languageCookie.Values["PageLanguage"] = GlobalConstants.USER_LANG_ARABIC; ;

            Response.Cookies.Remove("PageLanguage");

            Response.Cookies.Add(languageCookie);

            return RedirectToAction("Dashboard", "Tender");

        }

        

        [HttpPost]
        public JsonResult GetRivalMaster()
        {
            try
            {
                List<Rival> rival = _settingsProvider.GetRivalMaster();
                return Json(new { Result = "OK", Records = rival });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult AddRivalDetails(Rival rival)
        {
            if (!ModelState.IsValid)
            {
                return Json(rival);
            }

            bool IsAddedSuccess = _settingsProvider.AddRivalMaster(rival);

            if (IsAddedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        public JsonResult GetRivalByID(int id)
        {
            Rival rival = _settingsProvider.GetRivalMasterByID(id);
            return Json(rival);
        }

        [HttpPost]
        public JsonResult UpdateRival(Rival rival)
        {
            if (!ModelState.IsValid)
            {
                return Json(rival);
            }

            bool IsUpdatedSuccess = _settingsProvider.UpdateRivalMaster(rival);

            if (IsUpdatedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        //public ActionResult DeleteUser(int id)
        //{
        //    bool IsDeletedSuccessfully = _settingsProvider.DeleteUser(id);

        //    if (IsDeletedSuccessfully)
        //        Success("User Deleted Successfully");
        //    else
        //        Error(MessageConstants.UNEXPECTED_ERROR);

        //    return RedirectToAction("ManageUsers");
        //}


        public ActionResult WebSettings()
        {
            return View(_settingsProvider.GetWebSetting());
        }

        [HttpPost]
        public JsonResult GetCompanyLogoByID()
        {
            try
            {
                List<AddUser> users = _settingsProvider.GetUserDetails();
                return Json(new { Result = "OK", Records = users });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ManageUsers()
        {
            return View();
        }

        public ActionResult ManageRoles()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetRoleDetails()
        {
            try
            {
                List<Role> roles = _settingsProvider.GetRoleDetails();
                return Json(new { Result = "OK", Records = roles });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult GetUserDetails()
        {
            try
            {
                List<AddUser> users = _settingsProvider.GetUserDetails();
                return Json(new { Result = "OK", Records = users });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult AddUser(AddUser user)
        {
            if (!ModelState.IsValid)
            {
                return Json(user);
            }

            user.Password = AuthenticationHelper.EncryptPassword(user.Password);

            bool IsAddedSuccess = _settingsProvider.AddUser(user);

            if (IsAddedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        [HttpPost]
        public JsonResult CheckUserExistance(string userName)
        {
            bool IsUserExists = _settingsProvider.CheckUserExistance(userName);

            return Json(IsUserExists.ToString());
        }


        public JsonResult GetUserByID(int id)
        {
            AddUser user = _settingsProvider.GetUserByID(id);
            user.Password = AuthenticationHelper.DecryptPassowrd(user.Password);
            return Json(user);
        }

        [HttpPost]
        public JsonResult UpdateUser(UpdateUser user)
        {
            if (!ModelState.IsValid)
            {
                return Json(user);
            }

            user.Password = AuthenticationHelper.EncryptPassword(user.Password);

            bool IsUpdatedSuccess = _settingsProvider.UpdateUser(user);

            if (IsUpdatedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        public ActionResult DeleteUser(int id)
        {
            bool IsDeletedSuccessfully = _settingsProvider.DeleteUser(id);

            if (IsDeletedSuccessfully)
                Success("User Deleted Successfully");
            else
                Error(MessageConstants.UNEXPECTED_ERROR);

            return RedirectToAction("ManageUsers");
        }

        [HttpPost]
        public ActionResult UpdateSettings(HttpPostedFileBase file, string companyName)
        {
            if (file == null)
            {
                _settingsProvider.UpdateSettings(string.Empty,string.Empty,companyName);
            }
            else if (file.ContentLength > 0)
            {
                string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png",".jpeg"};

                int MaxContentLength = 1024 * 1024 * 3; //3 MB

                if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    Error("Please file of type: " + string.Join(", ", AllowedFileExtensions));
                }

                if (file.ContentLength > MaxContentLength)
                {
                    ViewBag.FileUploadMessage = "Your file is too large, maximum allowed size is: " + 200 + " MB";
                }
                else
                {
                    try
                    {
                        Random randomNumberGenerator = new Random();
                        var fileName = string.Format("{0}_{1}", randomNumberGenerator.Next(1, 32000), Path.GetFileName(file.FileName));
                        var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                        file.SaveAs(path);
                        ModelState.Clear();
                        bool isSuccess = _settingsProvider.UpdateSettings(fileName, path,companyName);
                        if (isSuccess)
                        {
                            Success("File Uploaded Successfully");
                            SettingHelper.ResetWebSetting();
                        }
                        else
                            Error(MessageConstants.UNEXPECTED_ERROR);
                    }
                    catch (Exception ex)
                    {
                        Error(MessageConstants.UNEXPECTED_ERROR);
                    }
                }
            }

            return RedirectToAction("WebSettings");

        }

    }
}
