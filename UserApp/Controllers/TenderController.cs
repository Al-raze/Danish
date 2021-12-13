using Infrastructure.Constants;
using Infrastructure.Core.DataAccess;
using Infrastructure.Core.Models;
using Infrastructure.Core.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AkraTechFramework.Helpers;
using AT.Core.Entities;
using AT.Core.Common;

namespace AkraTechFramework.Controllers
{
    [Authorize]
    public class TenderController : BaseController
    {
        //
        // GET: /Tender/

        private ITenderProvider _tenderProvider;
        //private GenericUserIdentity userIdentity;
        public TenderController(ITenderProvider tenderProvider)
        {
            this._tenderProvider = tenderProvider;
            //userIdentity = User.Identity.ToGenericUserIdentity();
        }

        public TenderController()
            : this(new TenderProvider(new TenderRepository()))
        {

        }

        public ActionResult Dashboard()
        {
            return View();
        }


        public ActionResult AddTender()
        {
            List<Company> company = _tenderProvider.GetCompanyDetails();
            ViewBag.Company = company;
            List<Tender> bank = _tenderProvider.GetBankNameDetails();
            ViewBag.BankName = bank;
            return View();
        }

        [HttpPost]
        public ActionResult AddTender(AddTender tender)
        {
            if (!ModelState.IsValid)
            {
                Error(MessageConstants.TENDER_ERROR);
                return View("AddTender", tender);

            }
            var userIdentity = User.Identity.ToGenericUserIdentity();
            tender.CreatedBy = userIdentity.UserID;

            bool isAddSuccess = _tenderProvider.AddTender(tender);


            if (isAddSuccess)
            {
                Success("Tender Added Successfully");
                return RedirectToAction("ModifyTender", "Tender");
            }
            else
            {
                Error(MessageConstants.UNEXPECTED_ERROR);
                return View("AddTender", tender);
            }
        }

        public ActionResult ModifyTender()
        {
            List<Tender> status = _tenderProvider.GetTenderStatusName();
            ViewBag.StatusName = status;
            return View();
        }

        public JsonResult GetTenderDetails(int StatusID)
        {
            try
            {
                List<Tender> tender = _tenderProvider.GetTenderDetails(StatusID);

                foreach (var item in tender)
                {
                    item.StatusValue = LanguageHelper.GetLabelCaption(item.StatusValue, "ModifyTender");
                }

                return Json(new { Result = "OK", Records = tender });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult EditTender(int id)
        {
            ModifyTender tender = _tenderProvider.GetTenderByID(id);
            string msg =Convert.ToString(TempData["FileDeleteMessage"]);
            if (!String.IsNullOrEmpty(msg))
            {
                ViewBag.FileUploadMessage = msg;
            }

            return View(tender);
        }

        [HttpPost]
        public ActionResult UpdateTender(ModifyTender tender)
        {
            //if (!ModelState.IsValid)
            //{
            //    Error(MessageConstants.TENDER_ERROR);
            //    return View("EditTender", tender);
            //}
            var userIdentity = User.Identity.ToGenericUserIdentity();
            tender.ModifiedBy = userIdentity.UserID;

            bool IsUpdateSuccess = _tenderProvider.UpdateTender(tender);

            if (IsUpdateSuccess)
            {
                Success("Tender Updated Successfully");
                return RedirectToAction("ModifyTender");
            }
            else
                Error(MessageConstants.UNEXPECTED_ERROR);
            return View("EditTender", tender);

        }

        public ActionResult DeleteTender(int id)
        {
            bool Isdeleted = _tenderProvider.DeleteTender(id);
            if (Isdeleted)
                Success("Tender Deleted Successfully");
            else
                Error(MessageConstants.UNEXPECTED_ERROR);

            return RedirectToAction("ModifyTender");
        }


        public JsonResult GetRivalDetails(int id)
        {
            try
            {
                List<Rival> rival = _tenderProvider.GetRivalDetails(id);
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

            bool IsAddedSuccess = _tenderProvider.CreateRival(rival);

            if (IsAddedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }
        public JsonResult GetRivalDetailsByID(int id)
        {
            Rival rival = _tenderProvider.GetRivalByID(id);
            return Json(rival);
        }


        [HttpPost]
        public JsonResult UpdateRivalDetails(Rival rival)
        {
            if (!ModelState.IsValid)
            {
                return Json(rival);
            }

            bool IsUpdatedSuccess = _tenderProvider.UpdateRivalDetails(rival);

            if (IsUpdatedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }


        public JsonResult GetUploadDoc(int id)
        {
            try
            {
                List<Tender> file = _tenderProvider.GetUploadDocumentFile(id);
                return Json(new { Result = "OK", Records = file });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, int tenderID)
        {

            if (file == null)
            {
                ModelState.AddModelError("File", "Please Upload Your file");
            }
            else if (file.ContentLength > 0)
            {
                int MaxContentLength = 1024 * 1024 * 200; //3 MB

                //Todo : Take this from settings 
                //string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf",".doc",".docx", };
                //if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                //{
                //    Error("Please file of type: " + string.Join(", ", AllowedFileExtensions));
                //}

                if (file.ContentLength > MaxContentLength)
                {
                    ViewBag.FileUploadMessage = "Your file is too large, maximum allowed size is: " + 200 + " MB";
                }
                else
                {
                    try
                    {
                        var fileName = string.Format("{0}_{1}", tenderID, Path.GetFileName(file.FileName));
                        var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                        file.SaveAs(path);
                        ModelState.Clear();
                        bool isSuccess = _tenderProvider.Uploadfile(fileName, path, tenderID);
                        if (isSuccess)
                            ViewBag.FileUploadMessage = "File Uploaded Successfully";
                        else
                            Error(MessageConstants.UNEXPECTED_ERROR);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.FileUploadMessage = "MessageConstants.UNEXPECTED_ERROR";
                    }

                }
            }

            return View("EditTender", _tenderProvider.GetTenderByID(tenderID));
        }

        public ActionResult DeleteUploadDoc(int fileID, int tenderID)
        {
            bool Isdeleted = _tenderProvider.DeleteUploadDoc(fileID);
            if (Isdeleted)
                TempData["FileDeleteMessage"] = "File Deleted Successfully";

            else
                Error(MessageConstants.UNEXPECTED_ERROR);

            return RedirectToAction("EditTender/" + tenderID);
        }

        public ActionResult DeleteRivals(int id)
        {
            bool isDeletedSuccess = _tenderProvider.DeleteRivals(id);
            if(isDeletedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }

        public ActionResult AddColor(int[] rivalID, string colorCode)
        {
            bool isAddedSuccess = _tenderProvider.AddColor(rivalID, colorCode);
            if (isAddedSuccess)
                return Json("Succeed");
            else
                return Json(MessageConstants.UNEXPECTED_ERROR);
        }
    }
}
