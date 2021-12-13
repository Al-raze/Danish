using AkraTechFramework.Controllers;
using Infrastructure.Constants;
using Infrastructure.Core.Provider;
using Infrastrucutre.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManager.Controllers
{
    [Authorize]
    public class ListingController : BaseController
    {
        //
        // GET: /Listing/

        IItemProvider _itemProvider;
        IListingProvider _listingProvider;
        public ListingController(IItemProvider itemProvider, IListingProvider listingProvider)
        {
            this._itemProvider = itemProvider;
            this._listingProvider = listingProvider;
        }

        public ListingController()
            : this(new ItemProvider(new ItemRepository()), new ListingProvider(new ListingRepository()))
        {

        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccountUpdate(int id)
        {
            var account = _listingProvider.GetSeller(id.ToString()).FirstOrDefault();

            return View(account);
        }

        public ActionResult UpdateAccount(SellerAccount account)
        {
            if (IsAdmin)
            {
                bool updated = _listingProvider.UpdateSellerAuth(account);

                if (updated)
                {
                    Success("Account Settings Updated");

                    ViewBag.IsUpdated = true;
                }
                else
                {
                    Error("Invalid Update Attempt");

                    ViewBag.IsUpdated = true;

                }
            }
            else
            {
                Error("Invalid Update Attempt");

                ViewBag.IsUpdated = true;
            }

            return View("AccountUpdate", account);
        }

        public ActionResult ListingSubmissions()
        {
            return View();
        }

        public ActionResult SubmitListing()
        {
            this.loadListingRequests();
            this.loadListingStatus();
            this.loadListingChannels();
            ViewBag.HeaderText = "Submit Listing";
            ViewBag.Mode = "Create";
            return View();
        }

        public ActionResult ListingRequest()
        {
            this.loadItemList();
            this.loadEbayCategoryItemList();
            this.loadAmazonCategoryItemList();
            ViewBag.HeaderText = "Add New Listing";
            ViewBag.Mode = "Create";
            return View(new ListingRequest() { RequestID = 0 }); 
        }



        [HttpPost]
        public JsonResult GetItemByID(int id)
        {
            ItemMaster item = _itemProvider.GetItemInfoByID(id);

            return Json(item);

        }

        public JsonResult GetListingRequests()
        {
            var items = _listingProvider.GetListingRequests();

            return Json(new { Result = "OK", Records = items });

        }

        public ActionResult StockLinkError()
        {
            loadPostage(); //Load Postage List 15-02-2021
            return View();
        }

        private void loadPostage(object selectedItem = null)
        {
            var PostalCarrierID = _itemProvider.GetPostage();

            ViewBag.PostalCarrierID = new SelectList(PostalCarrierID, "PostalCarrierID", "PostalCarrierName", selectedItem);
        }

        public JsonResult GetStockErrors()
        {
            var items = _listingProvider.GetStockErrors();

            return Json(new { Result = "OK", Records = items });
        }

        public JsonResult GetListingSubmissions(int id=0)
        {
            var items = _listingProvider.GetListingSubmissions(id);

            return Json(new { Result = "OK", Records = items });

        }

        public JsonResult GetItemSellerLink(int id = 0)
        {
            var items = _listingProvider.GetItemSellerLink(id);

            return Json(new { Result = "OK", Records = items });

        }

        [HttpPost]
        public JsonResult GetJSONListingRequestByID(int id)
        {
            var items = _listingProvider.GetListingRequests(id);

            return Json(items.FirstOrDefault());

        }


        public ActionResult GetListingRequestByID(int id)
        {
            ListingRequest request = _listingProvider.GetListingRequestByID(id);

            this.loadItemList(request.ItemMasterID);
            this.loadEbayCategoryItemList(request.EbayCategoryID);
            this.loadAmazonCategoryItemList(request.AmazonCategoryID);
            
            ViewBag.HeaderText = "Update Listing";
            ViewBag.Mode = "Update";

            return View("ListingRequest", request);
        }

        [HttpPost]
        public ActionResult InsertAndFix(SellerItemLink link)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    link.ModifiedDate = DateTime.Now;
                    link.ModifiedByUser = base.UserIdentity.UserID;

                    var result = _listingProvider.InsertAndFix(link);

                    return Json(new { Result = "OK" });
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "Item cannot be blank" }); ;
                }

                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }); ;
            }
        }


        public ActionResult UpdateSellerAccount(SellerAccount account)
        {
            try
            {
                var result = _listingProvider.UpdateSellerAccount(account);

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }); ;
            }

        }

        public ActionResult InsertSellerAccount(SellerAccount account)
        {
            try
            {
                var result = _listingProvider.InsertSellerAccount(account);

                return Json(new { Result = "OK", Record = result });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }); ;
            }

        }

        public JsonResult GetChannels()
        {

            var list = _listingProvider.GetListingChannels();

            var channels = new List<dynamic>();

            foreach (var item in list)
            {
                channels.Add(new { DisplayText = item.ListingChannelName , Value = item.ListingChannelID });
            }

            return Json(new { Result = "OK", Options = channels });
        }

        public JsonResult GetSellerItemLink(int id, string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0)
        {
            int rowCount = 0;
            var items =  _listingProvider.GetSellerItemLink(out rowCount, id, filter, filterText, jtStartIndex, jtPageSize);
            return Json(new { Result = "OK", Records = items, TotalRecordCount = rowCount, CurrentPage = jtStartIndex });
        }



        public ActionResult Accounts()
        {
            return View();
        }

        public JsonResult GetSellerAccounts()
        {
            var accounts = new List<SellerAccount>();

            if (IsAdmin)
            {
                accounts = _listingProvider.GetSellerAccounts();    
            }

            return Json(new { Result = "OK", Records = accounts });
        }

        public ActionResult ItemSellerLink()
        {
            ViewBag.Sellers = _listingProvider.GetSellerList();

            return View();
        }

        public ActionResult GetListingSubmissionByID(int id)
        {
            ListingSubmission submission = _listingProvider.GetListingSubmissionByID(id);

            this.loadListingRequests(submission.ListingRequestID);
            this.loadListingStatus(submission.ListingStatus);
            this.loadListingChannels(submission.ListingChannelID);

            ViewBag.HeaderText = "Update Submission";
            ViewBag.Mode = "Update";

            return View("SubmitListing", submission);
        }

        [HttpPost]
        public ActionResult UpdateLink(SellerItemLink link)
        {
            try
            {
                link.ModifiedDate = DateTime.Now;

                link.ModifiedByUser = UserIdentity.UserID;

                var result = _listingProvider.UpdateLink(link);

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }); ;
            }
        }
        [HttpPost]
        public ActionResult DeleteLink(int ItemLinkId)
        {
            try
            {
                var result = _listingProvider.DeleteLink(ItemLinkId);

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }); ;
            }
        }

        [HttpPost]
        public ActionResult InsertLink(SellerItemLink link)
        {
            try
            {
                link.ModifiedDate = DateTime.Now;

                link.ModifiedByUser = UserIdentity.UserID;

                var result = _listingProvider.InsertLink(link);

                return Json(new { Result = "OK", Record = result });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }); ;
            }
            
        }
        [HttpPost]
        public ActionResult UpdateSellerItemLink(SellerItemLink link)
        {
            _listingProvider.UpdateSellerItemLink(link);

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        public JsonResult GetRequestImages(int id)
        {
            try
            {
                List<ListingDocument> images = _listingProvider.GetListingRequestImages(id);
                return Json(new { Result = "OK", Records = images });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult DeleteUploadDoc(int fileID, int requestID)
        {
            try
            {
                _listingProvider.DeleteFile(requestID, fileID);
                return Json(new { Result = "OK"});
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            
                       
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, int RequestID)
        {

            string uploadMessage = string.Empty;
            
            string[] AllowedFileExtensions = new string[] { ".jpg",".jpeg", ".gif", ".png", ".pdf",".doc",".docx", };
            
            if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()))
            {
                uploadMessage = "Please select file of type: " + string.Join(", ", AllowedFileExtensions);

                return Content(uploadMessage);
            }


            if (file == null)
            {
                return Content("Please Upload Your file");
            }
            else if (file.ContentLength > 0)
            {
                int MaxContentLength = 1024 * 1024 * 200; //3 MB

                if (file.ContentLength > MaxContentLength)
                {
                    //ViewBag.FileUploadMessage = "Your file is too large, maximum allowed size is: " + 200 + " MB";
                    return Content("Your file is too large, maximum allowed size is: " + 200 + " MB");
                }
                else
                {
                    try
                    {
                        var fileName = string.Format("{0}_{1}", RequestID, Path.GetFileName(file.FileName));
                        var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                        file.SaveAs(path);
                        ModelState.Clear();
                        bool isSuccess = _listingProvider.Uploadfile(fileName, path, RequestID);
                        if (isSuccess)
                            uploadMessage =  "File Uploaded Successfully";
                        else
                            uploadMessage = MessageConstants.UNEXPECTED_ERROR;
                    }
                    catch (Exception ex)
                    {
                        uploadMessage = ex.Message; ;
                    }

                }
            }

            return Content(uploadMessage);
        }

        [HttpPost]
        public ActionResult AddListingSubmission(ListingSubmission listing, string command)
        {
            bool IsUpdated = false;
            bool IsCreated = false;

            if (!ModelState.IsValid)
            {
                this.loadListingRequests(listing.ListingRequestID);
                this.loadListingStatus(listing.ListingStatus);
                this.loadListingChannels(listing.ListingChannelID);
                Attention("Please verify the fields you have entered");
                return View("SubmitListing", listing);
            }

            if (command == "Save")
            {
                IsCreated = _listingProvider.SubmitListing(listing);
            }
            else if (command == "Update")
            {
                IsUpdated = _listingProvider.UpdateSubmission(listing);
            }


            if (IsCreated)
            {
                Success("Listing Submission Successfully Updated");
                return RedirectToAction("ListingSubmissions");
            }
            else if (IsUpdated)
            {
                Success("Listing Submission Successfully Updated");
                return RedirectToAction("ListingSubmissions");
            }
            else
            {
                this.loadListingRequests(listing.ListingRequestID);
                this.loadListingStatus(listing.ListingStatus);

                Attention("Please verify the fields you have entered");

                return View("SubmitListing", listing);
            }

        }

        [HttpPost]
        public ActionResult AddListing(ListingRequest listing, string command)
        {
            bool IsUpdated = false;
            bool IsCreated = false;

            if (!ModelState.IsValid)
            {
                Attention("Please verify the fields you have entered");
                return View("ListingRequest", listing);
            }
            
            if (command == "Save")
            {
                int listingRequestID = 0;

                IsCreated = _listingProvider.AddListing(listing, out listingRequestID);
            }
            else if (command == "Update")
            {
                IsUpdated = _listingProvider.UpdateListing(listing);
            }


            if (IsCreated)
            {
                Success("Listing Created Successfully.");
                return RedirectToAction("Index");
            }
            else if (IsUpdated)
            {
                Success("Listing Updated Successfully.");
                return RedirectToAction("Index");
            }
            else
            {
                this.loadItemList(listing.ItemMasterID);
                this.loadEbayCategoryItemList(listing.EbayCategoryID);
                this.loadAmazonCategoryItemList(listing.AmazonCategoryID);

                Attention("Please verify the fields you have entered");

                return View("ListingRequest", listing);
            }

        }

        private void loadItemList(object selectedItem = null)
        {
            var items = _itemProvider.GetItemNames();

            ViewBag.ItemMasterID = new SelectList(items, "ItemMasterID", "ItemName", selectedItem);
        }

        private void loadListingRequests(object selectedItem = null)
        {
            var items = _listingProvider.GetUnAllocatedListingRequestNo();


            ViewBag.ListingRequestID = new SelectList(items, "RequestID", "ListingRequestNo", selectedItem);
        }

        private void loadListingStatus(object selectedItem = null)
        {
            var items = _listingProvider.GetListingStatus();

            ViewBag.ListingStatus = new SelectList(items, "StatusDescription", "StatusCode", selectedItem);
        }


        private void loadListingChannels(object selectedItem = null)
        {
            var items = _listingProvider.GetListingChannels();

            ViewBag.ListingChannelID = new SelectList(items, "ListingChannelID", "ListingChannelName", selectedItem);
        }


        private void loadEbayCategoryItemList(object selectedItem = null)
        {
            var items = _itemProvider.GetEbayCategories();

            ViewBag.EbayCategoryID = new SelectList(items, "EbayCategoryID", "EbayCategoryName", selectedItem);
        }

        private void loadAmazonCategoryItemList(object selectedItem = null)
        {
            var items = _itemProvider.GetAmazonCategories();

            ViewBag.AmazonCategoryID = new SelectList(items, "AmazonCategoryID", "AmazonCategoryName", selectedItem);
        }

    }
}
