using Infrastrucutre.Core.Models;
using Infrastructure.Core.DataAccess;
using Infrastructure.Core.Provider;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastrucutre.Core.DataAccess;
using System.Xml;
using MarketplaceWebService.Model;
using System.IO;
using CsvHelper;
using System.Xml.Linq;
using System.Runtime.InteropServices;

namespace Inventory.Synchronizer
{
    public class AmazonSync
    {
        private ListingProvider _listingProvider;
        TimeZoneInfo britishTimezone;
        public delegate int InsertOrderDelegate(string details);
        public event InsertOrderDelegate InsertOrderEvent;

        private SellerAccount _settings = null;

        private SellerAccount Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = _listingProvider.GetSeller(this.SellerIndex).FirstOrDefault();
                }

                return _settings;
            }
        }

        private string SellerIndex { get; set; }

        public AmazonSync(string index)
        {
            this._listingProvider = new ListingProvider(new ListingRepository());
            this.britishTimezone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            this.SellerIndex = index;
        }

        public void DownloadOrderReport(DateTime startDate, DateTime toDate)
        {
            MarketplaceWebService.MarketplaceWebServiceConfig config = new MarketplaceWebService.MarketplaceWebServiceConfig();
            config.SetUserAgentHeader(
                "SyncrhonizeApp",
                "1.1",
                "C#");
            config.ServiceURL = "https://mws.amazonservices.co.uk";

            MarketplaceWebService.MarketplaceWebService service = new MarketplaceWebService.MarketplaceWebServiceClient(Settings.AccessKey, Settings.AuthenticationToken, config);
            GetReportListRequest request = new GetReportListRequest();
            request.Merchant = Settings.SellerIdKey;
            TypeList list = new TypeList();
            list.Type = new List<string>();
            list.Type.Add("_GET_FLAT_FILE_ORDERS_DATA_");


            request.ReportTypeList = list;
            request.AvailableFromDate = startDate.Date; //DateTime.Now.AddDays(-7);
            request.AvailableToDate = toDate.ToEndOfDay();

            GetReportListResponse response = service.GetReportList(request);

            GetReportListResult getReportListResult = response.GetReportListResult;

            List<ReportInfo> reportInfoList = getReportListResult.ReportInfo;

            if (reportInfoList.Count < 1)
            {
                InsertOrderEvent(string.Format("No Reports to sync"));
                return;
            }

            var distinctReport = reportInfoList.Select(r => r.ReportId).Distinct();

            List<string> reports = new List<string>();


            for (int i = 0; i < reportInfoList.Count; i++)
            {
                try
                {
                    string reportName = string.Format("Reports/{0}{1}.txt", reportInfoList[i].ReportId, DateTime.Now.ToFileTimeUtc());

                    reports.Add(reportName);
                    InsertOrderEvent(string.Format("Downloading Report Available On {2}-{0}-{1}", reportName, reportInfoList[i].ReportType, reportInfoList[i].AvailableDate));
                    SyncReport(reportInfoList[i].ReportId, reportName);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("throttled"))
                    {
                        InsertOrderEvent(string.Format("Throttling Wait One Minute..Retry {0}", reportInfoList[i].ReportId));
                        i--;
                        System.Threading.Thread.Sleep(1000 * 60);
                    }
                    else
                    {
                        InsertOrderEvent(string.Format("Error Syncronizing orders - ", ex.Message));
                    }

                }
            }
        }

        public void GetReportRequests(DateTime startDate, DateTime toDate)
        {
            MarketplaceWebService.MarketplaceWebServiceConfig config = new MarketplaceWebService.MarketplaceWebServiceConfig();
            config.SetUserAgentHeader(
                "SyncrhonizeApp",
                "1.1",
                "C#");
            config.ServiceURL = "https://mws.amazonservices.co.uk";

            MarketplaceWebService.MarketplaceWebService service = new MarketplaceWebService.MarketplaceWebServiceClient(Settings.AccessKey, Settings.AuthenticationToken, config);
            GetReportListRequest request = new GetReportListRequest();
            request.Merchant = Settings.SellerIdKey;
            TypeList list = new TypeList();
            list.Type = new List<string>();
            list.Type.Add("_GET_V2_SETTLEMENT_REPORT_DATA_XML_");

            request.ReportTypeList = list;
            request.AvailableFromDate = startDate; //DateTime.Now.AddDays(-7);
            request.AvailableToDate = toDate;

            GetReportListResponse response = service.GetReportList(request);

            GetReportListResult getReportListResult = response.GetReportListResult;

            List<ReportInfo> reportInfoList = getReportListResult.ReportInfo;



            if (reportInfoList.Count < 1)
            {
                InsertOrderEvent(string.Format("No Reports to sync"));
                return;
            }


            var distinctReport = reportInfoList.Select(r => r.ReportId).Distinct();



            List<string> reports = new List<string>();



            foreach (var item in reportInfoList)
            {
                string reportName = string.Format("Reports/{0}{1}.xml", item.ReportId, DateTime.Now.ToFileTimeUtc());

                reports.Add(reportName);
                InsertOrderEvent(string.Format("Downloading Report Available On {2}-{0}-{1}", reportName, item.ReportType, item.AvailableDate));
                SyncReport(item.ReportId, reportName);

            }

            foreach (var item in reports)
            {
                InsertOrderEvent(string.Format("Synchronizing Report {0}", item));
                ParseCsvFileNewFormat(item);
            }



            InsertOrderEvent(string.Format("Report Sychronization From {0} to {1} Complete ", startDate.ToShortDateString(), startDate.AddDays(7).ToShortDateString()));
        }

        public void SyncReport(string reportId, string reportName)
        {
            MarketplaceWebService.MarketplaceWebServiceConfig config = new MarketplaceWebService.MarketplaceWebServiceConfig();
            config.SetUserAgentHeader(
                "SyncrhonizeApp",
                "1.1",
                "C#");
            config.ServiceURL = "https://mws.amazonservices.co.uk";

            MarketplaceWebService.MarketplaceWebService service = new MarketplaceWebService.MarketplaceWebServiceClient(Settings.AccessKey, Settings.AuthenticationToken, config);

            GetReportRequest reportRequest = new GetReportRequest();

            reportRequest.ReportId = reportId;
            reportRequest.Merchant = Settings.SellerIdKey;
            reportRequest.WithReportId(reportId);
            reportRequest.Report = File.Open(reportName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            GetReportResponse response = service.GetReport(reportRequest);
            GetReportResult getReportResult = response.GetReportResult;
            reportRequest.Report.Close();
            reportRequest.Report.Dispose();

        }

        public int TryParseOrFail(string value)
        {
            int result = 0;

            int.TryParse(value, out result);

            return result;
        }

        public void ParseCsvFileNewFormat(string filePath)
        {
            XElement xmlDoc = XElement.Load(filePath);

            var customers = from cust in xmlDoc.Descendants("Order")
                            select new SaleOrder
                            {
                                OrderID = cust.Element("AmazonOrderID").Value,
                                ShipmentID = cust.Element("ShipmentID").Value,
                                MarketplaceName = cust.Element("MarketplaceName").Value,
                                OrderItems = new List<SaleOrderItem>(from t in cust.Descendants("Item")
                                                                     select new SaleOrderItem
                                                                     {
                                                                         ItemCode = t.Element("AmazonOrderItemCode").Value,
                                                                         SKU = t.Element("SKU").Value,
                                                                         Quantity = TryParseOrFail(t.Element("Quantity").Value),
                                                                         ItemFees = new List<SalePrice>(
                                                                             from p in t.Descendants("Fee")
                                                                             select new SalePrice
                                                                             {
                                                                                 Type = p.Element("Type").Value,
                                                                                 Amount = p.Element("Amount").Value

                                                                             }),
                                                                         ItemPrice = new List<SalePrice>(
                                                                             from p in t.Descendants("ItemPrice")
                                                                                        .Descendants("Component")
                                                                             select new SalePrice
                                                                             {
                                                                                 Type = p.Element("Type").Value,
                                                                                 Amount = p.Element("Amount").Value

                                                                             })
                                                                     })
                            };

            var salesList = customers.ToList();

            string errorMessage = string.Empty;

            foreach (var item in salesList)
            {
                _listingProvider.InsertSalesCommission(item, out errorMessage);
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    InsertOrderEvent(errorMessage);
                }
            }


        }

        public void AmazonSynchronizeImages()
        {
            List<ItemImage> itemCodes = _listingProvider.GetItemCodes(Settings.SellerID);

            MarketplaceWebServiceProducts.MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProducts.MarketplaceWebServiceProductsConfig();
            config.ServiceURL = "https://mws.amazonservices.co.uk/Products/2011-10-01";

            MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient productsClient = new MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient("SyncronizeImages", "1.1", Settings.AccessKey, Settings.AuthenticationToken, config);
            MarketplaceWebServiceProducts.Model.GetMatchingProductForIdRequest request = new MarketplaceWebServiceProducts.Model.GetMatchingProductForIdRequest();
            request.SellerId = Settings.SellerIdKey;
            request.MarketplaceId = Settings.MarketPlaceId;

            for (int i = 0; i < itemCodes.Count; i += 4)
            {
                List<ItemImage> itemImages = new List<ItemImage>();


                MarketplaceWebServiceProducts.Model.IdListType idList = new MarketplaceWebServiceProducts.Model.IdListType() { Id = itemCodes.Skip(i).Take(4).Select(t => t.ItemID).ToList() };
                request.WithIdList(idList);
                request.IdType = "ASIN";

                var products = productsClient.GetMatchingProductForId(request);


                if (products.GetMatchingProductForIdResult.Count > 0)
                {
                    foreach (var item in products.GetMatchingProductForIdResult)
                    {
                        try
                        {
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(item.ToXML());
                            string imageUrl = document.GetElementsByTagName("ns2:URL").Item(0).InnerText;
                            string itemID = document.GetElementsByTagName("ASIN").Item(0).InnerText;
                            itemImages.Add(new ItemImage() { ImageUrl = imageUrl, ItemID = itemID, SellerID = Settings.SellerID });
                        }
                        catch
                        {

                            InsertOrderEvent(string.Format("Error Synchronizing Item ID- {0}", item.Id));
                            InsertOrderEvent(string.Format(""));
                        }

                    }

                    _listingProvider.InsertImages(itemImages);
                }

            }

        }

        public List<List<T>> SplitList<T>(List<T> me, int size = 50)
        {
            var list = new List<List<T>>();
            for (int i = 0; i < me.Count; i += size)
                list.Add(me.GetRange(i, Math.Min(size, me.Count - i)));
            return list;
        }


        public void ParseOrderFile(string fileName, bool refreshItemsOnly = false)
        {
            try
            {
                List<string> orderNumbers = new List<string>();
                using (var reader = new StreamReader(fileName))
                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.RegisterClassMap<OrderFileMap>();

                    while (csv.Read())
                    {
                        orderNumbers.Add(csv[0].Split('\t')[0]);
                    }
                }

                var list = SplitList(orderNumbers, 20);

                for (int i = 0; i < list.Count; i++)
                {
                    try
                    {
                        InsertOrderEvent(string.Format("Syncing Batch {0}/{1}", (i + 1), list.Count));
                        AmazonSyncOrderNo(list[i], refreshItemsOnly);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("throttled"))
                        {
                            i--;
                            InsertOrderEvent(string.Format("Throttling Wait One Minute.."));
                            System.Threading.Thread.Sleep(1000 * 60);
                        }
                        else
                        {
                            InsertOrderEvent(string.Format("Error Syncronizing orders - ", ex.Message));
                        }

                    }
                }

                InsertOrderEvent(string.Format("Synchronization Complete"));

                //foreach (var orderNumber in orderNumbers)
                //{


                //}
            }
            catch (Exception ex)
            {

                InsertOrderEvent(string.Format("Error loading order file {0} - {1}", fileName, ex.Message));
            }
        }

        public void AmazonSyncOrderNo(List<string> orderNumbers, bool refreshItemsOnly = false)
        {
            MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig();

            config.ServiceURL = "https://mws.amazonservices.co.uk/Orders/2013-09-01";

            MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersClient ordersClient = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersClient(Settings.AccessKey, Settings.AuthenticationToken, "SyncrhonizeApp", "1.1", config);

            MarketplaceWebServiceOrders.Model.GetOrderRequest request = new MarketplaceWebServiceOrders.Model.GetOrderRequest();

            request.SellerId = Settings.SellerIdKey;

            request.AmazonOrderId = orderNumbers;

            var orders = ordersClient.GetOrder(request);

            if (orders.GetOrderResult.Orders.Count > 0)
            {
                if (refreshItemsOnly)
                {
                    SynchronizeAmazonOrderItem(orders.GetOrderResult.Orders);
                }
                else
                {
                    SynchronizeAmazonOrders(orders.GetOrderResult.Orders);
                }

            }
            else
            {
                InsertOrderEvent(string.Format("Orders Not Found"));
            }

        }


        public void AmazonSynchronization(string startDate, string endDate, List<string> marketplace)
        {
            //MarketplaceWebServiceOrders;
            //MarketplaceWebServiceOrders.Model;

            InsertOrderEvent(string.Format("Synchronizing for Seller {0}", this.Settings.SellerID));

            MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig();

            config.ServiceURL = "https://mws.amazonservices.co.uk/Orders/2013-09-01";


            MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersClient ordersClient = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersClient(Settings.AccessKey, Settings.AuthenticationToken, "SyncrhonizeApp", "1.1", config);
            MarketplaceWebServiceOrders.Model.ListOrdersRequest request = new MarketplaceWebServiceOrders.Model.ListOrdersRequest();

            request.FulfillmentChannel = new List<string>() { "MFN", "AFN" };
            request.SellerId = Settings.SellerIdKey;

            var startDateTimeFilter = DateTime.SpecifyKind(DateTime.Parse(startDate), DateTimeKind.Utc);

            var endDateTimeFilter = DateTime.Parse(endDate);

            if (endDateTimeFilter.Date == DateTime.Now.Date)
            {
                endDateTimeFilter = DateTime.Now.ToUniversalTime().AddMinutes(-5);
            }
            else
            {
                endDateTimeFilter = DateTime.SpecifyKind(DateTime.Parse(startDate).ToEndOfDay(), DateTimeKind.Utc);
            }

            InsertOrderEvent(string.Format("Synchronizing Orders between UTC Time {0} and {1}  ", startDateTimeFilter,endDateTimeFilter));
            
            request.CreatedAfter = startDateTimeFilter;//TimeZoneInfo.ConvertTime(DateTime.Parse(startDate).AddDays(-1), TimeZoneInfo.Local, britishTimezone);
            request.CreatedBefore = endDateTimeFilter;// DateTime.Parse(endDate).AddHours(DateTime.Now.Hour); //TimeZoneInfo.ConvertTime(DateTime.Parse(endDate), TimeZoneInfo.Local, britishTimezone);
            request.OrderStatus = new List<string>(){
                "Unshipped", "PartiallyShipped", "Shipped"
            };            
            
            request.MarketplaceId = marketplace;

            var orders = ordersClient.ListOrders(request);

            if (orders.ListOrdersResult.Orders.Count > 0)
            {
                InsertOrderEvent(string.Format("Synchronizing {0} Orders", orders.ListOrdersResult.Orders.Count));
                SynchronizeAmazonOrders(orders.ListOrdersResult.Orders);

                if (orders.ListOrdersResult.IsSetNextToken())
                {
                    MarketplaceWebServiceOrders.Model.ListOrdersByNextTokenRequest newRequest = new MarketplaceWebServiceOrders.Model.ListOrdersByNextTokenRequest();
                    newRequest.NextToken = orders.ListOrdersResult.NextToken;
                    newRequest.SellerId = Settings.SellerIdKey;
                    MarketplaceWebServiceOrders.Model.ListOrdersByNextTokenResponse orderResponse = new MarketplaceWebServiceOrders.Model.ListOrdersByNextTokenResponse();

                    do
                    {
                        var result = ordersClient.ListOrdersByNextToken(newRequest);

                        InsertOrderEvent(string.Format("Synchronizing {0} Orders", result.ListOrdersByNextTokenResult.Orders.Count));  //orders.ListOrdersResult.Orders.Count

                        SynchronizeAmazonOrders(result.ListOrdersByNextTokenResult.Orders);

                        if (!result.ListOrdersByNextTokenResult.IsSetNextToken())
                        {
                            break;
                        }
                        else
                        {
                            newRequest.NextToken = result.ListOrdersByNextTokenResult.NextToken;
                        }

                    } while (true);

                }

            }

        }


        private bool SynchronizeAmazonOrderItem(List<MarketplaceWebServiceOrders.Model.Order> orders)
        {
            //List<Order> eBayOrders = new List<Order>();
            foreach (MarketplaceWebServiceOrders.Model.Order order in orders)
            {
                List<OrderItem> orderLineItems = new List<OrderItem>();  //GetAmazonOrderLineItems(order.AmazonOrderId, sellerAccount);

                if (order.OrderTotal != null && order.OrderStatus != "Canceled")
                {
                    //eBayOrders.Add();
                    //order.IsSetFulfillmentChannel()

                    try
                    {
                        InsertOrderEvent(string.Format("Synchronizing Order - {0},Status {1}", order.AmazonOrderId, order.OrderStatus));

                        var orderHeader = new Order();

                        orderHeader.OrderItems = GetAmazonOrderLineItems(order.AmazonOrderId);

                        if (order.OrderType != null)
                        {
                            orderHeader.OrderType = order.OrderType;
                        }

                        _listingProvider.RefreshOrderItems(order.AmazonOrderId, orderHeader.OrderItems);

                    }
                    catch (Exception ex)
                    {
                        InsertOrderEvent(string.Format("Error Synchronizing Order - {0},Status - {1}", order.AmazonOrderId, order.OrderStatus));
                        InsertOrderEvent(string.Format(""));
                    }

                }

            }

            return true;

        }


        private bool SynchronizeAmazonOrders(List<MarketplaceWebServiceOrders.Model.Order> orders)
        {
            //List<Order> eBayOrders = new List<Order>();
            foreach (MarketplaceWebServiceOrders.Model.Order order in orders)
            {
                List<OrderItem> orderLineItems = new List<OrderItem>();  //GetAmazonOrderLineItems(order.AmazonOrderId, sellerAccount);

                if (order.OrderTotal != null && order.OrderStatus != "Canceled")
                {
                    //eBayOrders.Add();
                    //order.IsSetFulfillmentChannel()

                    try
                    {
                        InsertOrderEvent(string.Format("Synchronizing Order - {0},Status {1}", order.AmazonOrderId, order.OrderStatus));

                        var orderHeader = new Order();

                        orderHeader.AmountPaid = double.Parse(order.OrderTotal.Amount);

                        if (order.BuyerEmail != null)
                        {
                            orderHeader.BuyerUserID = order.BuyerEmail.Split('@').FirstOrDefault();
                        }

                        if (order.ShippingAddress != null)
                        {
                            if (order.ShippingAddress.Name != null)
                            {
                                orderHeader.Name = order.ShippingAddress.Name;
                            }

                            if (order.ShippingAddress.AddressLine1 != null)
                            {
                                orderHeader.Street1 = order.ShippingAddress.AddressLine1;
                            }

                            if (order.ShippingAddress.AddressLine2 != null)
                            {
                                orderHeader.Street2 = order.ShippingAddress.AddressLine2;
                            }

                            if (order.ShippingAddress.City != null)
                            {
                                orderHeader.CityName = order.ShippingAddress.City;
                            }

                            if (order.ShippingAddress.StateOrRegion != null)
                            {
                                orderHeader.StateOrProvince = order.ShippingAddress.StateOrRegion;
                            }

                            if (order.ShippingAddress.Phone != null)
                            {
                                orderHeader.Phone = order.ShippingAddress.Phone;
                            }

                            if (order.ShippingAddress.PostalCode != null)
                            {
                                orderHeader.PostalCode = order.ShippingAddress.PostalCode;
                            }

                            if (order.ShippingAddress.CountryCode != null)
                            {
                                orderHeader.Country = order.ShippingAddress.CountryCode;
                            }

                        }

                        orderHeader.OrderItems = _listingProvider.CheckExistingOrder(order.AmazonOrderId) ? orderLineItems : GetAmazonOrderLineItems(order.AmazonOrderId);

                        if (order.FulfillmentChannel != null)
                        {
                            var xml = order.ToXML();

                            xml.ToString();
                            //orderHeader.OrderType =     //o rder.FulfillmentChannel;
                        }

                        orderHeader.OrderReferenceNo = order.AmazonOrderId;
                        //30 th March to 26th October + hr                    
                        orderHeader.OrderDate = order.PurchaseDate;
                        orderHeader.OrderStatus = order.OrderStatus.ToString();
                        orderHeader.SellerID = Settings.SellerID;
                        orderHeader.SellerIndex = Settings.SellerIndex;
                        orderHeader.CheckoutState = order.OrderStatus;
                        orderHeader.ShippedDate = order.OrderStatus == "Shipped" ? GetShipDate(order.LatestShipDate) : null;
                        orderHeader.SalesOrderNumber = order.AmazonOrderId;
                        orderHeader.IsActive = true;
                        _listingProvider.InsertOrders(new List<Order>() { orderHeader });

                    }
                    catch (Exception ex)
                    {
                        InsertOrderEvent(string.Format("Error Synchronizing Order - {0},Status - {1}", order.AmazonOrderId, order.OrderStatus));
                        InsertOrderEvent(string.Format(""));
                    }

                }

            }

            return true;

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

        private List<OrderItem> GetAmazonOrderLineItems(string amazonOrderId)
        {
            List<OrderItem> orderLineItems = new List<OrderItem>();
            MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig();

            config.ServiceURL = "https://mws.amazonservices.co.uk/Orders/2013-09-01";


            MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersClient ordersClient = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersClient(Settings.AccessKey, Settings.AuthenticationToken, "SyncrhonizeApp", "1.1", config);
            MarketplaceWebServiceOrders.Model.ListOrderItemsRequest request = new MarketplaceWebServiceOrders.Model.ListOrderItemsRequest();
            request.AmazonOrderId = amazonOrderId;
            request.SellerId = Settings.SellerIdKey;

            var transactions = ordersClient.ListOrderItems(request);

            foreach (var item in transactions.ListOrderItemsResult.OrderItems)
            {

                orderLineItems.Add(new OrderItem()
                {

                    ItemID = item.ASIN,
                    ItemTitle = item.Title,
                    Quantity = int.Parse(item.QuantityOrdered.ToString()),
                    ShippingCost = item.ShippingPrice == null ? 0.00 : double.Parse(item.ShippingPrice.Amount),
                    SKUItemID = item.SellerSKU,
                    TransactionPrice = item.ItemPrice == null ? 0.00 : double.Parse(item.ItemPrice.Amount)
                });
            }

            return orderLineItems;



        }
    }
}
