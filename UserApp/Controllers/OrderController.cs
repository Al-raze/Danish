using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using eBay.Service.Util;
using System.Configuration;
using AkraTechFramework.Controllers;
using Infrastrucutre.Core.Models;
using Infrastructure.Core.Provider;
using Infrastrucutre.Core.DataAccess;
using InventoryManager.CustomFilters;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Constants;
using System.Threading;
using RestSharp;
using MWSClientCsRuntime;
using System.IO;
using System.Xml;
using Inventory.Synchronizer;
using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Ajax.Utilities;

namespace InventoryManager.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        //
        // GET: /Order/

        ApiContext apiContext;

        TimeZoneInfo britishTimezone;

        IListingProvider _listingProvider;
        IItemProvider _itemProvider;


        public OrderController(IListingProvider listingProvider, IItemProvider itemProvider)
        {

            this._listingProvider = listingProvider;
            this.britishTimezone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            this._itemProvider = itemProvider;
        }

        public OrderController()
            : this(new ListingProvider(new ListingRepository()), new ItemProvider(new ItemRepository()))
        {

        }

        public ActionResult EditOrders(string selectedorders)
        {

            if (selectedorders.ElementAt(0) == ',')
                selectedorders = selectedorders.Remove(0, 1);

            int[] orderIds = (selectedorders.Split(',').Select(t => int.Parse(t)).ToArray());

            var orders = _listingProvider.GetListingOrders(orderIds);

            loadCarriersList();

            return View("EditOrders", orders);

        }

        public ActionResult UpdateOrdersCommon(/*List<OrderItem> orderLineItem,*/ List<Order> orderPostalcarrier)
        {
            //var updateLineItem = _listingProvider.UpdateOrders(orderLineItem);
            var updatePostalcarrier = _listingProvider.UpdateOrderCarriers(orderPostalcarrier);

            return Json(new { Success = true });
        }
        public ActionResult UpdateOrders(List<OrderItem> orderItems)
        {
            var update = _listingProvider.UpdateOrders(orderItems);

            return Json(new { Success = update });
        }

        //public ActionResult UpdateOrderCarriers(List<Order> orders)
        //{
        //    var update = _listingProvider.UpdateOrderCarriers(orders);

        //    return Json(new { Success = update });
        //}

        public JsonResult GetOrders(string startDate = null, string endDate = null, string sellerId = null, string dispatchStatus = null, string orderNumber = null, bool? specialDelivery = null, string jtSorting = null)
        {
            try
            {
                if (string.IsNullOrEmpty(startDate))
                {
                    return Json(new { Result = "OK", Records = new List<Order>() });
                }
                var orders = _listingProvider.GetListingOrders(startDate: startDate, endDate: endDate, sellerId: sellerId, dispatchStatus: dispatchStatus, orderNumber: orderNumber, specialDelivery: specialDelivery, jtSorting: jtSorting);

                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public ActionResult SalesReport()
        {
            loadSellersList();
            return View();
        }
        //-----------------------------------------------------------------------SKU Report------------------------------------------------------------------------------//
        public JsonResult GetSKUReport(string startDate, string endDate, string Asin, string SKU, string ItemMasterID, string SellerID, string jtSorting = null)
        {
            try
            {
                var orders = _itemProvider.GetSKUReport(startDate, endDate, Asin, SKU,ItemMasterID,SellerID);



                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        orders = orders
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        orders = orders
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult Get_SKUReport(string startDate, string endDate, string Asin,string SKU, string SellerID, string jtSorting = null) //SKU Sub Data Repor
        {
            try
            {
                var SkuData = _itemProvider.Get_SKUReport(startDate, endDate, Asin,SKU,SellerID);



                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        SkuData = SkuData
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        SkuData = SkuData
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = SkuData });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult Get_AsinReport(string startDate, string endDate, string Asin, string ItemMasterID ,string jtSorting = null) //Asin Report
        {
            try
            {
                var orders = _itemProvider.Get_AsinReport(startDate, endDate, Asin, ItemMasterID);



                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        orders = orders
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        orders = orders
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult Get_ReportByItem(string startDate, string endDate, string Asin, string jtSorting = null) //Report Get_ReportByItem 
        {
            try
            {
                var orders = _itemProvider.Get_AsinSubReport(startDate, endDate,Asin);

                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        orders = orders
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        orders = orders
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult Get_ReportItemSummary(string startDate, string endDate, string Asin, string jtSorting = null) //Report Get_ReportItemSummary 
        {
            try
            {
                var orders = _itemProvider.Get_ReportItemSummary(startDate, endDate, Asin);



                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        orders = orders
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        orders = orders
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        //------------------------------------------------Group Summary ----------------------------//

        public JsonResult Get_GroupByReport(string startDate, string endDate, string UserId, string jtSorting = null) //Report Get_ReportItemSummary 
        {
            try
            {
                var orders = _itemProvider.Get_GroupByReport(startDate, endDate, UserId);



                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        orders = orders
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        orders = orders
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult Get_GroupSummary(string startDate, string endDate, string UserId, string jtSorting = null) //Report Get_ReportItemSummary 
        {
            try
            {
                var orders = _itemProvider.Get_GroupSummary(startDate, endDate, UserId);



                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        orders = orders
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        orders = orders
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult Get_SumGroupReport(string startDate, string endDate, string UserId, string jtSorting = null) //Report Get_ReportItemSummary 
        {
            try
            {
                var orders = _itemProvider.Get_SumGroupReport(startDate, endDate, UserId);



                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        orders = orders
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        orders = orders
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------//
        public JsonResult GetSalesReport(string startDate, string endDate, string jtSorting = null)
        {
            try
            {
                var orders = _itemProvider.GetSalesReport(startDate, endDate);



                if (jtSorting != null)
                {
                    string[] sort = jtSorting.Split(' ');

                    if (sort[1].Equals("ASC", StringComparison.OrdinalIgnoreCase))
                    {
                        orders = orders
                       .OrderBy(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }
                    else
                    {
                        orders = orders
                                .OrderByDescending(r => r.GetType().GetProperty(sort[0]).GetValue(r, null)).ToList();
                    }

                }


                return Json(new { Result = "OK", Records = orders });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpGet]
        public FileContentResult GenereateYodelDelivery(string selectedvalues)
        {
            if (selectedvalues.ElementAt(0) == ',')
                selectedvalues = selectedvalues.Remove(0, 1);

            int[] items = (selectedvalues.Split(',').Select(t => int.Parse(t)).ToArray());

            var orders = _listingProvider.GetListingOrders(items);


            MemoryStream memoryStream = new MemoryStream();

            using (TextWriter writer = new StreamWriter(memoryStream))
            {
                CsvHelper.CsvWriter csvWriter = new CsvHelper.CsvWriter(writer);

                csvWriter.WriteHeader<YodelImport>();

                foreach (var item in orders)
                {
                    YodelImport record = new YodelImport();
                    record.AccountID = item.AccountNumber;
                    record.TariffCode = item.Code;
                    record.YourReference = item.OrderReferenceNo;
                    record.Del_Company = string.Empty;
                    record.Del_Address1 = item.Street1;
                    record.Del_Address2 = item.Street2;
                    record.Del_Address3 = string.Empty;
                    record.Del_Town = item.CityName;
                    record.Del_County = item.StateOrProvince;
                    record.Del_PostCode = item.PostalCode;
                    record.Del_Contact_1 = item.Name;
                    record.Del_Phone = item.Phone;
                    record.Del_Email = string.Empty;
                    record.Pieces = item.OrderItems.Select(t => t.Quantity).Sum().ToString();
                    record.Weight = string.Empty;
                    record.Customs_Data = string.Empty;
                    string itemNames = string.Empty;
                    item.OrderItems.Select(r => r.ItemTitle + " " + r.AdditionalInfo).ToList().ForEach(t => itemNames += t);
                    record.Notes = itemNames;
                    csvWriter.WriteRecord(record);

                }

                writer.Flush();

                return File(memoryStream.ToArray(), "text/csv", string.Format("YodelDelivery-{0}.csv", DateTime.Now.ToString("yyyyMMdd-HHmmss")));

            }

        }

        [HttpGet]
        public FileContentResult GenereateSpecialDelivery(string selectedvalues)
        {
            if (selectedvalues.ElementAt(0) == ',')
                selectedvalues = selectedvalues.Remove(0, 1);

            int[] items = (selectedvalues.Split(',').Select(t => int.Parse(t)).ToArray());

            var orders = _listingProvider.GetListingOrders(items);


            MemoryStream memoryStream = new MemoryStream();

            using (TextWriter writer = new StreamWriter(memoryStream))
            {
                CsvHelper.CsvWriter csvWriter = new CsvHelper.CsvWriter(writer);

                csvWriter.WriteHeader<AddressImport>();

                foreach (var item in orders)
                {
                    AddressImport record = new AddressImport();
                    record.ContactName = item.Name;
                    record.Country = "United Kingdom";
                    record.PostalCode = item.PostalCode;
                    record.Address1 = item.Street1;
                    record.Address2 = item.Street2;
                    record.Town = item.CityName;
                    record.County = item.StateOrProvince;
                    record.TelephoneNumber = item.Phone;
                    string itemNames = string.Empty;
                    item.OrderItems.Select(r => r.ItemTitle + " " + r.AdditionalInfo).ToList().ForEach(t => itemNames += t);
                    record.CustomerRef = itemNames;
                    record.AlternativeRef = item.OrderReferenceNo;
                    record.Items = item.OrderItems.Count.ToString();
                    record.Weight = "1";

                    csvWriter.WriteRecord(record);

                }

                writer.Flush();

                return File(memoryStream.ToArray(), "text/csv", string.Format("SpecialDelivery-{0}.csv", DateTime.Now.ToString("yyyyMMdd-HHmmss")));

            }

        }


        public ContentResult SynchronizeImages(string sellerId)
        {
            var sellerAccount = _listingProvider.GetSeller(sellerId).FirstOrDefault();


            switch (sellerAccount.ChannelName.ToUpper())
            {
                case GlobalConstants.CHANNEL_AMAZON:
                    SyncAmazonImages(sellerAccount);
                    break;
                default:
                    break;
            }

            return Content("OK");


        }

        private void SyncAmazonImages(SellerAccount account)
        {
            var sync = new AmazonSync(account.SellerIndex.ToString());
            sync.InsertOrderEvent += sync_InsertOrderEvent;
            sync.AmazonSynchronizeImages();
        }

        private void AmazonSynchronization(string startDate, string endDate, SellerAccount account)
        {
            var sync = new AmazonSync(account.SellerIndex.ToString());
            sync.InsertOrderEvent += sync_InsertOrderEvent;
            sync.AmazonSynchronization(startDate, endDate, new List<string>());
        }


        int sync_InsertOrderEvent(string details)
        {
            //Log if required
            return 0;
        }


        public ContentResult SynchronizeOrders(string startDate, string endDate, string sellerId)
        {

            var sellerAccount = _listingProvider.GetSeller(sellerId).FirstOrDefault();


            switch (sellerAccount.ChannelName.ToUpper())
            {
                case GlobalConstants.CHANNEL_AMAZON:
                    AmazonSynchronization(startDate, endDate, sellerAccount);
                    break;
                case GlobalConstants.CHANNEL_EBAY:
                    EBaySynchronization(startDate, endDate, sellerAccount);
                    break;
                
                default:
                    break;
            }

            return Content("OK");

        }

        private void EBaySynchronization(string startDate, string endDate, SellerAccount sellerAccount)
        {
            TimeFilter fltr = new TimeFilter();

            GetOrdersCall apicall = new GetOrdersCall(GetApiContext(sellerAccount));


            //fltr.TimeFrom = DateTime.Now.AddDays(-20);
            //fltr.TimeTo = DateTime.Now.AddDays(1);

            apicall.CreateTimeFrom = TimeZoneInfo.ConvertTime(DateTime.Parse(startDate).AddDays(-1), TimeZoneInfo.Local, britishTimezone);
            apicall.CreateTimeTo = TimeZoneInfo.ConvertTime(DateTime.Parse(endDate).AddDays(1), TimeZoneInfo.Local, britishTimezone);

            //apicall.OrderIDList = new StringCollection(new string[] { "458" });

            apicall.Execute();  //.GetOrders(fltr, TradingRoleCodeType.Seller,OrderStatusCodeType.All);

            List<Order> eBayOrders = new List<Order>();

            if (apicall.ApiResponse.Ack != AckCodeType.Failure)
            {
                if (apicall.ApiResponse.OrderArray.Count != 0)
                {
                    int pageNumber = 1;
                    int totalPageCount = apicall.PaginationResult.TotalNumberOfPages;

                    SynchronizeEBayOrders(apicall.ApiResponse.OrderArray, sellerAccount);

                    while (apicall.HasMoreOrders)
                    {
                        var page = new PaginationType();
                        page.PageNumber = pageNumber++;
                        apicall.Pagination = page;
                        apicall.Execute();
                        SynchronizeEBayOrders(apicall.ApiResponse.OrderArray, sellerAccount);
                    }


                }
            }

        }

        private bool SynchronizeEBayOrders(OrderTypeCollection orders, SellerAccount sellerAccount)
        {
            List<Order> eBayOrders = new List<Order>();
            foreach (OrderType order in orders)
            {
                List<OrderItem> orderLineItems = GetOrderLineItems(order.TransactionArray.Cast<TransactionType>().ToList());

                eBayOrders.Add(new Order()
                {
                    AmountPaid = order.AmountPaid.Value,
                    BuyerUserID = order.BuyerUserID,
                    Name = order.ShippingAddress.Name,
                    Street1 = order.ShippingAddress.Street1,
                    Street2 = order.ShippingAddress.Street2,
                    CityName = order.ShippingAddress.CityName,
                    StateOrProvince = order.ShippingAddress.StateOrProvince,
                    Country = order.ShippingAddress.CountryName,
                    Phone = order.ShippingAddress.Phone,
                    OrderItems = orderLineItems,
                    PostalCode = order.ShippingAddress.PostalCode,
                    OrderReferenceNo = order.OrderID,
                    //30 th March to 26th October + hr                    
                    OrderDate = TimeZoneInfo.ConvertTime(order.CreatedTime, TimeZoneInfo.Utc, britishTimezone),
                    OrderStatus = order.OrderStatus.ToString(),
                    SellerID = order.SellerUserID,
                    CheckoutState = order.CheckoutStatus.Status.ToString(),
                    ShippedDate = GetShipDate(order.ShippedTime),
                    //SalesOrderNumber = order.OrderID,
                    SalesOrderNumber = order.ShippingDetails.SellingManagerSalesRecordNumber.ToString(),
                    Channel = sellerAccount.ChannelName,
                    SellerIndex = sellerAccount.SellerIndex,
                    IsActive = true
                });

            }

            return _listingProvider.InsertOrders(eBayOrders);

        }

        private List<OrderItem> GetOrderLineItems(List<TransactionType> transactions)
        {
            List<OrderItem> orderLineItems = new List<OrderItem>();

            foreach (var item in transactions)
            {

                string sku = "N/A";

                if (item.Item.SKU == null)
                {
                    if (item.Variation != null)
                    {
                        sku = item.Variation.SKU.Trim();
                    }
                }
                else
                {
                    sku = item.Item.SKU.Trim();
                }


                orderLineItems.Add(new OrderItem()
                {
                    ItemID = item.Item.ItemID,
                    ItemTitle = item.Item.Title,
                    Quantity = item.QuantityPurchased,
                    ShippingCost = item.ActualShippingCost == null ? 0 : item.ActualShippingCost.Value,
                    TransactionPrice = item.TransactionPrice.Value,
                    SKUItemID = sku
                });
            }

            return orderLineItems;

        }

        private DateTime? GetShipDate(DateTime shipDate)
        {
            if (shipDate == DateTime.MinValue)
            {
                return null;
            }
            else
            {
                return shipDate;
            }

        }


        private void loadSellersList()
        {
            var sellers = _listingProvider.GetSeller(sync: true);
            ViewBag.SellerID = new SelectList(sellers, "SellerIndex", "SellerID");
        }

        private void loadCarriersList()
        {
            var carrierProvider = new CarrierProvider(new CarrierRepository());

            var carriers = carrierProvider.GetCarriers();

            ViewBag.PostalCarrierID = new SelectList(carriers, "PostalCarrierID", "PostalCarrierName");
        }


        public ActionResult InvoiceMaster()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PrintInvoice(string selectedvalues)
        {
            if (selectedvalues.ElementAt(0) == ',')
                selectedvalues = selectedvalues.Remove(0, 1);

            int[] items = (selectedvalues.Split(',').Select(t => int.Parse(t)).ToArray());

            return View("Invoice", _listingProvider.GetListingOrders(items, jtSorting: "Images"));

        }


        public ActionResult Index()
        {
            loadCarriersList();
            loadSellersList();
            ViewBag.OrderDownloadFormat = loadDownloadFormat();
            return View();
        }

        

        //-------------------------------------------Implementation Of Reports And List--------------------------------
        public ActionResult AsinReport() //Asin Report 25-10-2020
        {
            loadSellersList();
            loadUserList();
            loadItemList();
            //loadAsinsList();
            loadSkuList();
            //loadSKUList(ItemMasterID);
            return View();
        }

        public ActionResult SKUReport() //SKU Report 25-10-2020
        {
            loadSellersList();
            loadItemList();
            loadSkuList();
            loadUserList();
            //loadSKUList(ItemMasterID);
            //ViewBag.OrderDownloadFormat = loadDownloadFormat();
            return View();
        }

        public ActionResult ItemReport() //Asin Report 25-10-2020
        {
            loadUserList();
            loadItemList();
            
            return View();
        }

        public ActionResult GroupBy() //Group By Report 30-11-2021
        {
            loadGroupUserList();
            

            return View();
        }

        public ActionResult SaleReport() //Sale Report 25-10-2020
        {
            return View();
        }

        private void loadItemList() //Items List
        {
            var ItemMasterID = _listingProvider.GetItemsList();
            ViewBag.ItemMasterID = new SelectList(ItemMasterID, "ItemMasterID", "ItemName");

        }

        public void loadSkuList() //SKU List
        {
            var SKU = _listingProvider.GetSKUList();
            ViewBag.StockId = new SelectList(SKU, "ItemMasterID", "SKU");

        }

        [HttpGet]
        public JsonResult LookupItem(string searchText, bool onlyItemName = false) //Item Searching Sugg 18-02-2021
        {
            var items = _itemProvider.GetItemNames(searchText, onlyItemName);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LookupSupplier(string searchText) //Supplier Searching Sugg 18-02-2021
        {
            var items = _itemProvider.GetSupplierNames(searchText);

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        //-------------------------------------------Get Asin No AND SKU AND Seller -----------------------//

        
        public JsonResult GetASINListById(int ItemMasterID) // Get Item List By User Group 25-02-2021
        {

            var AsinList = _itemProvider.GetAsinByID(ItemMasterID).Where(x => x.ItemMasterID == ItemMasterID).Select(x => new
            {
                ItemMasterID = x.ItemMasterID.ToString(),
                ListingItemNo = x.ListingItemNo.ToString(),
            }).ToList();

            return Json(AsinList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetSKUListById(string ListingItemNo)
        {

            var SKUList = _itemProvider.GetSKUByID(ListingItemNo).Where(x => x.ListingItemNo == ListingItemNo).Select(x => new
            {
                ListingItemNo = x.ListingItemNo,
                SKU = x.SKU
            }).ToList();


            //return new JsonResult()
            //{
            //    Data = SKUList,
            //    MaxJsonLength = 86753090
            //};
            return Json(SKUList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetItemsListById(int SellerID)
        {
            var ItemsList = _itemProvider.GetItemsListByID(SellerID).Where(x => x.SellerIndex == SellerID).Select(x => new
            {
                ItemMasterID = x.ItemMasterID,
                ItemName = x.ItemName
            }).ToList();

            
            return Json(ItemsList, JsonRequestBehavior.AllowGet);
        }

        //-------------------------------------------------------------------------//

        private void loadAsinsList(object selectedItem = null) //Get Asin List 
        {
            var Asin = _listingProvider.GetAsinsList();
            ViewBag.StockId = new SelectList(Asin, "ListingItemNo", "ListingItemNo",selectedItem);
        }


       //---------------------------------------28-10-2020---------------------------------------------------------------//

        public ApiContext GetApiContext(SellerAccount sellerAccount)
        {
            //apiContext is a singleton,
            //to avoid duplicate configuration reading
            if (apiContext != null)
            {
                return apiContext;
            }
            else
            {
                apiContext = new ApiContext();

                //set Api Server Url
                apiContext.SoapApiServerUrl =
                    ConfigurationManager.AppSettings["Environment.ApiServerUrl"];
                //set Api Token to access eBay Api Server
                ApiCredential apiCredential = new ApiCredential();



                apiCredential.eBayToken = sellerAccount.AuthenticationToken;
                //ConfigurationManager.AppSettings["UserAccount.ApiToken"];
                apiContext.ApiCredential = apiCredential;
                //set eBay Site target to US
                apiContext.Site = SiteCodeType.UK;

                ////set Api logging
                //apiContext.ApiLogManager = new ApiLogManager();
                //apiContext.ApiLogManager.ApiLoggerList.Add(
                //    new FileLogger("ErrorLog\\listing_log.txt", true, true, true)
                //    );
                //apiContext.ApiLogManager.EnableLogging = true;


                return apiContext;
            }
        }


        public ContentResult UpdateCarrier(int PostalCarrierID, int[] OrderIDs)
        {
            DateTime proceedDateTime = DateTime.Now;
            if (OrderIDs.Length > 0)
            {
                _listingProvider.UpdateCarrier(PostalCarrierID, proceedDateTime, OrderIDs);
                return Content("OK");
            }
            else
            {
                return Content("not OK");
            }
        }
        public ContentResult updateReactiveDeletedOrders(int[] OrderIDs)
        {
            DateTime proceedDateTime = DateTime.Now;
            if (OrderIDs.Length > 0)
            {
                _listingProvider.updateReactiveDeletedOrders(proceedDateTime, OrderIDs);
                return Content("OK");
            }
            else
            {
                return Content("not OK");
            }
        }

        public ContentResult UpdateCarrierRef(string carrierRef, int[] OrderIDs)
        {
            DateTime proceedDateTime = DateTime.Now;
            if (OrderIDs.Length > 0)
            {
                _listingProvider.UpdateCarrierRef(carrierRef, proceedDateTime, OrderIDs);
                return Content("OK");
            }
            else
            {
                return Content("not OK");
            }
        }
        public ContentResult UpdateAdditionalNotes(string AditionalNotes, int[] OrderIDs)
        {
            if (OrderIDs.Length > 0)
            {
                _listingProvider.UpdateAdditionalNotes(AditionalNotes, OrderIDs);
                return Content("OK");
            }
            else
            {
                return Content("not OK");
            }
        }


        [HttpPost]
        public JsonResult DeleteOrder(int OrderID)
        {
            if (_listingProvider.DeleteOrder(OrderID))
                return Json(new { Result = "OK" });
            else
                return Json(new { Result = "ERROR", Message = "UnExpected Error" });

        }


        private DataTable SetOrderDataInDatatable(int formatID, string downloadFormatName, List<Order> orders)
        {
            int serial = 1;
            //Creating DataTable  
            DataTable dt = new DataTable();
            //Setiing Table Name  
            dt.TableName = "CustomerOrderList";
            if (formatID == 1)//Royal Mail
            {
                //Add Columns  
                dt.Columns.Add("OrderID", typeof(string));
                dt.Columns.Add("First_Name", typeof(string));
                dt.Columns.Add("Delivery_phone", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Reference", typeof(string));
                dt.Columns.Add("Weight(Kg)");
                dt.Columns.Add("Carrier", typeof(string));
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Total Price", typeof(string));
                dt.Columns.Add("Address_line_1", typeof(string));
                dt.Columns.Add("Address_line_2", typeof(string));
                dt.Columns.Add("Address_line_3", typeof(string));
                dt.Columns.Add("Address_line_4", typeof(string));
                dt.Columns.Add("Address_line_5", typeof(string));
                dt.Columns.Add("Postcode", typeof(string));
                dt.Columns.Add("Post To Country", typeof(string));

                foreach (var order in orders.OrderBy(x => x.SalesOrderNumber))
                {
                    //foreach (var orderItem in order.OrderItems)
                    //{
                    string orderid = order.OrderID.ToString();
                    if (order.SellerID.Trim().ToLower() == "salsabil_euro" || order.SellerID.Trim().ToLower() == "smaya_sales")
                    {
                        orderid = order.SalesOrderNumber;
                    }
                    double weight = Convert.ToDouble(order.Weight);
                    //string weightAfterFormat = String.Format("%.2f", weight);
                    string weightAfterFormat = string.Format("{0:0.00}", weight);
                    dt.Rows.Add(orderid, order.Name, order.Phone, "", /*order.OrderItems[0].CarrierRef*/order.CarrierRef ?? order.OrderItems[0].SKUItemID, weightAfterFormat, downloadFormatName, /*order.OrderItems.Sum(x=>x.Quantity)*/1,
                        "Â£" + order.OrderItems.Sum(x => x.TransactionPrice), order.Street1,
                        order.Street2, "", order.CityName, order.StateOrProvince, order.PostalCode, order.Country);
                    dt.AcceptChanges();
                    //}
                }

            }
            else if (formatID == 2)//Parcel Force Euro
            {
                //Add Columns  
                dt.Columns.Add("Address Code", typeof(string));
                dt.Columns.Add("Full Name", typeof(string));
                dt.Columns.Add("ADDRESS line 1", typeof(string));
                dt.Columns.Add("ADDRESS line 2", typeof(string));
                dt.Columns.Add("County", typeof(string));
                dt.Columns.Add("POST CODE", typeof(string));
                dt.Columns.Add("TOWN", typeof(string));
                dt.Columns.Add("Email Address", typeof(string));
                dt.Columns.Add("Mobile", typeof(string));
                dt.Columns.Add("CarreRef", typeof(string));
                dt.Columns.Add("Order No", typeof(string));
                dt.Columns.Add("Country Code", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string)); 
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                //dt.Columns.Add("", typeof(string));
                foreach (var order in orders)
                {
                    //foreach (var orderItem in order.OrderItems)
                    //{
                        dt.Rows.Add(serial, order.Name, order.Street1,"", order.StateOrProvince, order.PostalCode,
                                    order.CityName, "contact@arsuk.co.uk", order.Phone, order.CarrierRef ?? order.OrderItems[0].SKUItemID, order.OrderID, order.Country);

                        dt.AcceptChanges();
                    //}
                    serial++;
                }
            }
            else if (formatID == 3)//MyHermes
            {
                //Add Columns  
                dt.Columns.Add("Address_line_1", typeof(string));
                dt.Columns.Add("Address_line_2", typeof(string));
                dt.Columns.Add("Address_line_3", typeof(string));
                dt.Columns.Add("Postcode", typeof(string));
                dt.Columns.Add("full name", typeof(string));
                dt.Columns.Add("Weight(Kg)", typeof(string));
                dt.Columns.Add("Signature(y/n)", typeof(string));
                dt.Columns.Add("Reference", typeof(string));
                dt.Columns.Add("Parcel_value(£)", typeof(string)); 
                foreach (var order in orders)
                {
                    double weight = Convert.ToDouble(order.Weight);
                    //string weightAfterFormat = String.Format("%.2f", weight);
                    string weightAfterFormat = string.Format("{0:0.00}", weight);

                    dt.Rows.Add(order.Street1, order.StateOrProvince, order.CityName, order.PostalCode,
                                order.Name, weightAfterFormat, "N", order.CarrierRef, order.AmountPaid);

                    dt.AcceptChanges(); 
                    serial++;
                }




                ////Add Columns  
                //dt.Columns.Add("Address_line_1", typeof(string));
                //dt.Columns.Add("Address_line_2", typeof(string));
                //dt.Columns.Add("Address_line_3", typeof(string));
                //dt.Columns.Add("Address_line_4", typeof(string));
                //dt.Columns.Add("POSTCODE", typeof(string));
                //dt.Columns.Add("First_name", typeof(string));
                //dt.Columns.Add("Last_name", typeof(string));
                //dt.Columns.Add("Email", typeof(string));
                //dt.Columns.Add("Weight(Kg)", typeof(string));
                //dt.Columns.Add("Compensation(£)", typeof(string));
                //dt.Columns.Add("Signature(y/n)", typeof(string));
                //dt.Columns.Add("Reference", typeof(string));
                //dt.Columns.Add("Contents", typeof(string));
                //dt.Columns.Add("Parcel_value(£)", typeof(string));
                //dt.Columns.Add("Delivery_phone", typeof(string));
                //dt.Columns.Add("Delivery_safe_place", typeof(string));
                //dt.Columns.Add("Delivery_instructions", typeof(string));
                //dt.Columns.Add("Service", typeof(string));
                //foreach (var order in orders)
                //{
                //    foreach (var orderItem in order.OrderItems)
                //    {
                //        dt.Rows.Add(order.Street1, order.Street2, order.StateOrProvince, order.CityName,
                //                    order.PostalCode, order.Name, "", "", order.Weight, "(£)" + order.Price, "",
                //                    order.OrderReferenceNo, "", "(£)" + order.OrderItems.Sum(x => x.TransactionPrice).ToString(), order.Phone, "", "", "");

                //        dt.AcceptChanges();
                //    }
                //}
            }
            else if (formatID == 4)//DPD
            {

            }
            else if (formatID == 5)//UK mail
            {

            }
            if (formatID == 6)//Others
            {
                dt.Columns.Add("OrderID", typeof(string));
                dt.Columns.Add("First_Name", typeof(string));
                dt.Columns.Add("Delivery_phone", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Reference", typeof(string));
                dt.Columns.Add("Weight(Kg)", typeof(string));
                dt.Columns.Add("Carrier", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("Total Price", typeof(string));
                dt.Columns.Add("Address_line_1", typeof(string));
                dt.Columns.Add("Address_line_2", typeof(string));
                dt.Columns.Add("Address_line_3", typeof(string));
                dt.Columns.Add("Address_line_4", typeof(string));
                dt.Columns.Add("Address_line_5", typeof(string));
                dt.Columns.Add("Postcode", typeof(string));
                dt.Columns.Add("Post To Country", typeof(string));

                foreach (var order in orders)
                {
                    //foreach (var orderItem in order.OrderItems)
                    //{
                    dt.Rows.Add(order.OrderID, order.Name, order.Phone, "", /*order.OrderItems[0].CarrierRef*/order.CarrierRef ?? order.OrderItems[0].SKUItemID, order.Weight, downloadFormatName, /*order.OrderItems.Sum(x=>x.Quantity)*/1,
                        "Â£" + order.OrderItems.Sum(x => x.TransactionPrice), order.Street1,
                        order.Street2, "", order.CityName, order.StateOrProvince, order.PostalCode, order.Country);
                    dt.AcceptChanges();
                    //}
                }

            }
            return dt;
        }

        //public class OrderMemoryLinks
        //{
        //    public string FileGuid { get; set; }
        //    public string FileName { get; set; }
        //}

        [HttpPost]
        public ActionResult DownloadOrderList(int downloadFormatID, string downloadFormatName, int[] orderIds)
        {
            if (orderIds.Count() > 0)
            {
                //OrderMemoryLinks orderMemoryLinks = new OrderMemoryLinks();
                var _orderList = _listingProvider.GetListingOrders(orderIds);

                DataTable dt = SetOrderDataInDatatable(downloadFormatID, downloadFormatName, _orderList);
                //Name of File  
                string fileName = string.Format("{0}_CustomerOrderList-{1}.csv", DateTime.Now.ToString("yyyyMMdd-HH-mm-ss"), downloadFormatName);

                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Add DataTable in worksheet  
                    wb.Worksheets.Add(dt);

                    //var sheet = wb.Worksheets.FirstOrDefault(); 
                    wb.Worksheets.FirstOrDefault().Column(6).CellsUsed().SetDataType(XLCellValues.Text);
                    wb.Worksheets.FirstOrDefault().Column(8).CellsUsed().SetDataType(XLCellValues.Text);
                    wb.Worksheets.FirstOrDefault().Cell(1, 6).DataType = XLCellValues.Text;
                    wb.Worksheets.FirstOrDefault().Cell(1, 8).DataType = XLCellValues.Text;

                    string handle = Guid.NewGuid().ToString();
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        wb.SaveAs(memoryStream);
                        memoryStream.Position = 0;
                        Session[handle] = memoryStream.ToArray();
                    }
                    return new JsonResult()
                    {
                        Data = new { FileGuid = handle, FileName = fileName }
                    };
                }
            }
            else
            {
                return View("Index");
            }
        }

        //[HttpPost]
        //public ActionResult DownloadOrderListMultiple(int downloadFormatID, int[] orderIds)
        //{
        //    if (orderIds.Count() > 0)
        //    {
        //        string alllinks = "";
        //        List<OrderMemoryLinks> lstOrderMemoryLinks = new List<OrderMemoryLinks>();
        //        var _orderList = _listingProvider.GetListingOrders(orderIds);
        //        var orderListByGroupByPostalCarrier = _orderList.Where(x => x.PostalCarrierID != null).GroupBy(x => x.PostalCarrierID).ToList();
        //        foreach (var oneByOneOrderListByCarrier in orderListByGroupByPostalCarrier)
        //        {
        //            DataTable dt = SetOrderDataInDatatable(downloadFormatID, oneByOneOrderListByCarrier.ToList());
        //            //Name of File  
        //            string fileName = string.Format("CustomerOrderList-{0}-{1}.xlsx", oneByOneOrderListByCarrier.FirstOrDefault().PostalCarrierName, DateTime.Now.ToString("yyyyMMdd-HH-mm-ss"));

        //            using (XLWorkbook wb = new XLWorkbook())
        //            {
        //                //Add DataTable in worksheet  
        //                wb.Worksheets.Add(dt);

        //                string handle = Guid.NewGuid().ToString();
        //                using (MemoryStream memoryStream = new MemoryStream())
        //                {
        //                    wb.SaveAs(memoryStream);
        //                    memoryStream.Position = 0;
        //                    Session[handle] = memoryStream.ToArray();
        //                }
        //                lstOrderMemoryLinks.Add(new OrderMemoryLinks() { FileGuid = handle, FileName = fileName });
        //                //alllinks += "http://localhost:52845/Order/Download?fileGuid=" + handle + "&filename=" + fileName + "";
        //            }
        //        }
        //        return new JsonResult()
        //        {
        //            Data = new { lstOrderMemoryLinks = lstOrderMemoryLinks }
        //        };
        //    }
        //    else
        //    {
        //        return View("Index");
        //    }
        //}

        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (Session[fileGuid] != null)
            {
                byte[] data = Session[fileGuid] as byte[];
                //Session[fileGuid] = null;
                return File(data, "text/html", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }

        private SelectList loadDownloadFormat()
        {
            var selectList = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem {Text = "Royal Mail", Value = "1"},
                new SelectListItem {Text = "Parcel Force", Value = "2"},
                new SelectListItem {Text = "My Hermes", Value = "3"},
                new SelectListItem {Text = "DPD", Value = "4"},
                new SelectListItem {Text = "UK Mail", Value = "5"},
                new SelectListItem {Text = "Others", Value = "6"},
            }, "Value", "Text");
            return selectList;
        }


        //--------------------------Item Group User Code -------------------------------//

        private void loadGroupUserList() // load Group User List
        {
            
            var UserID = _listingProvider.GetGroupUser();

            ViewBag.UserID = new SelectList(UserID, "UserID", "UserName");
        }

        public ActionResult ItemGroupUser() //Item Group User
        {
            loadUserList();
            ViewBag.OrderDownloadFormat = loadDownloadFormat();
            return View();
        }
        private void loadUserList()
        {
            //var Users = _listingProvider.GetUser(sync: true);

            var UserID = _listingProvider.GetUser();

            ViewBag.UserID = new SelectList(UserID, "UserID", "UserName");
        }



        public JsonResult GetItems(string id, string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0)
        {
            int rowCount = 0;
            var items = _itemProvider.GetItems(out rowCount, id, filter, filterText, jtStartIndex, jtPageSize);
            return Json(new { Result = "OK", Records = items, TotalRecordCount = rowCount, CurrentPage = jtStartIndex });
        }

        public ContentResult AddGroup(int GID, int[] OrderIDs)
        {
               DateTime proceedDateTime = DateTime.Now;
            
                _listingProvider.AddGroup(GID, proceedDateTime, OrderIDs);
                return Content("OK");
            
        }
       
    }
}
