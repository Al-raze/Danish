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

namespace AmazonSynchronizer
{


    public class Synchronization
    {
        private ListingProvider _listingProvider;
        TimeZoneInfo britishTimezone;
        public delegate int InsertOrderDelegate(string details);
        public event InsertOrderDelegate InsertOrderEvent;

        public Synchronization()
        {
            this._listingProvider = new ListingProvider(new ListingRepository());
            this.britishTimezone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        }

        public void GetReportRequests(DateTime startDate,DateTime toDate)
        {
            MarketplaceWebService.MarketplaceWebServiceConfig config = new MarketplaceWebService.MarketplaceWebServiceConfig();
            config.SetUserAgentHeader(
                "SyncrhonizeApp",
                "1.1",
                "C#");
            config.ServiceURL = "https://mws.amazonservices.co.uk";

            MarketplaceWebService.MarketplaceWebService service = new MarketplaceWebService.MarketplaceWebServiceClient(Settings.AccessKey, Settings.AuthenticationToken, config);
            GetReportListRequest request = new GetReportListRequest();
            request.Merchant = Settings.SellerKey;
            TypeList list = new TypeList();
            list.Type = new List<string>();
            list.Type.Add("_GET_V2_SETTLEMENT_REPORT_DATA_XML_");

            request.ReportTypeList = list;
            request.AvailableFromDate = startDate; //DateTime.Now.AddDays(-7);
            request.AvailableToDate = toDate;

            GetReportListResponse response = service.GetReportList(request);

            GetReportListResult getReportListResult = response.GetReportListResult;

            List<ReportInfo> reportInfoList = getReportListResult.ReportInfo;

          

            if (reportInfoList.Count <1 )
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
                InsertOrderEvent(string.Format("Downloading Report Available On {2}-{0}-{1}",reportName,item.ReportType,item.AvailableDate));
                SyncReport(item.ReportId, reportName);
                               
            }

            foreach (var item in reports)
            {
                InsertOrderEvent(string.Format("Synchronizing Report {0}", item));               
                ParseCsvFileNewFormat(item);
            }



            InsertOrderEvent(string.Format("Report Sychronization From {0} to {1} Complete ", startDate.ToShortDateString() , startDate.AddDays(7).ToShortDateString() ));
        }

        public void SyncReport(string reportId,string reportName)
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
            reportRequest.Merchant = Settings.SellerKey;
            reportRequest.WithReportId(reportId);
            reportRequest.Report = File.Open(reportName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            GetReportResponse response = service.GetReport(reportRequest);
            GetReportResult getReportResult = response.GetReportResult;
            reportRequest.Report.Close();
            reportRequest.Report.Dispose();

        }

        public int TryParseOrFail(string value)
        {
            int result =0;

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
                                                                      Quantity =TryParseOrFail(t.Element("Quantity").Value),
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

            this.Settings = _listingProvider.GetSeller(this.SellerIndex).FirstOrDefault();

            MarketplaceWebServiceProducts.MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProducts.MarketplaceWebServiceProductsConfig();
            config.ServiceURL = "https://mws.amazonservices.co.uk/Products/2011-10-01";
            MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient productsClient = new MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient("SyncronizeImages", "1.1", Settings.AccessKey, Settings.AuthenticationToken, config);
            MarketplaceWebServiceProducts.Model.GetMatchingProductForIdRequest request = new MarketplaceWebServiceProducts.Model.GetMatchingProductForIdRequest();

            for (int i = 0; i < itemCodes.Count; i += 4)
            {
                List<ItemImage> itemImages = new List<ItemImage>();

                request.SellerId = Settings.SellerKey;
                request.MarketplaceId = Settings.MarketplaceId;
                MarketplaceWebServiceProducts.Model.IdListType idList = new MarketplaceWebServiceProducts.Model.IdListType() { Id = itemCodes.Skip(i).Take(4).Select(t => t.ItemID).ToList() };
                request.WithIdList(idList);
                request.IdType = "ASIN";

                var products = productsClient.GetMatchingProductForId(request);


                if (products.GetMatchingProductForIdResult.Count > 0)
                {
                    foreach (var item in products.GetMatchingProductForIdResult)
                    {
                        XmlDocument document = new XmlDocument();
                        document.LoadXml(item.ToXML());
                        string imageUrl = document.GetElementsByTagName("ns2:URL").Item(0).InnerText;
                        string itemID = document.GetElementsByTagName("ASIN").Item(0).InnerText;
                        itemImages.Add(new ItemImage() { ImageUrl = imageUrl, ItemID = itemID, SellerID = Settings.SellerID });
                    }

                    _listingProvider.InsertImages(itemImages);
                }

            }

        }


        public void AmazonSynchronization(string startDate, string endDate)
        {
            //MarketplaceWebServiceOrders;
            //MarketplaceWebServiceOrders.Model;

            MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig();

            config.ServiceURL = "https://mws.amazonservices.co.uk/Orders/2013-09-01";



            MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersClient ordersClient = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersClient(Settings.AccessKey, Settings.AuthenticationToken, "SyncrhonizeApp", "1.1", config);
            MarketplaceWebServiceOrders.Model.ListOrdersRequest request = new MarketplaceWebServiceOrders.Model.ListOrdersRequest();



            request.SellerId = Settings.SellerKey;
            request.CreatedAfter = TimeZoneInfo.ConvertTime(DateTime.Parse(startDate).AddDays(-1), TimeZoneInfo.Local, britishTimezone);
            request.CreatedBefore = DateTime.Parse(endDate).AddHours(DateTime.Now.Hour); //TimeZoneInfo.ConvertTime(DateTime.Parse(endDate), TimeZoneInfo.Local, britishTimezone);
            request.MarketplaceId = new List<string>() { Settings.MarketplaceId };

            //apicall.CreateTimeFrom = TimeZoneInfo.ConvertTime(DateTime.Parse(startDate).AddDays(-1), TimeZoneInfo.Local, britishTimezone);
            //apicall.CreateTimeTo = TimeZoneInfo.ConvertTime(DateTime.Parse(endDate).AddDays(1), TimeZoneInfo.Local, britishTimezone);


            var orders = ordersClient.ListOrders(request);          

            if (orders.ListOrdersResult.Orders.Count > 0)
            {
                InsertOrderEvent(string.Format("Synchronizing {0} Orders", orders.ListOrdersResult.Orders.Count));
                SynchronizeAmazonOrders(orders.ListOrdersResult.Orders);

                if (orders.ListOrdersResult.IsSetNextToken())
                {
                    MarketplaceWebServiceOrders.Model.ListOrdersByNextTokenRequest newRequest = new MarketplaceWebServiceOrders.Model.ListOrdersByNextTokenRequest();
                    newRequest.NextToken = orders.ListOrdersResult.NextToken;
                    newRequest.SellerId = Settings.SellerKey;
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


                    _listingProvider.SynchronizeSKU();
                }

            }

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
                    
                        _listingProvider.InsertOrders(new List<Order>() { new Order()
                        {
                            
                            AmountPaid = double.Parse(order.OrderTotal.Amount),
                            BuyerUserID = order.BuyerEmail.Split('@').FirstOrDefault(),
                            Name = order.ShippingAddress.Name,
                            Street1 = order.ShippingAddress.AddressLine1,
                            Street2 = order.ShippingAddress.AddressLine2,
                            CityName = order.ShippingAddress.City,
                            StateOrProvince = order.ShippingAddress.StateOrRegion,
                            Phone = order.ShippingAddress.Phone,
                            OrderItems = _listingProvider.CheckExistingOrder(order.AmazonOrderId) ? orderLineItems : GetAmazonOrderLineItems(order.AmazonOrderId),
                            PostalCode = order.ShippingAddress.PostalCode,
                            OrderReferenceNo = order.AmazonOrderId,
                            //30 th March to 26th October + hr                    
                            OrderDate = order.PurchaseDate,
                            OrderStatus = order.OrderStatus.ToString(),
                            SellerID = Settings.SellerID,
                            CheckoutState = order.OrderStatus,
                            ShippedDate = order.OrderStatus == "Shipped" ? GetShipDate(order.LatestShipDate) : null,
                            SalesOrderNumber = order.AmazonOrderId,
                            Country = order.ShippingAddress.CountryCode
                        }
                        });
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
            request.SellerId = Settings.SellerKey;

            var transactions = ordersClient.ListOrderItems(request);

            foreach (var item in transactions.ListOrderItemsResult.OrderItems)
            {
                
                orderLineItems.Add(new OrderItem()
                {
                    
                    ItemID = item.ASIN,
                    ItemTitle = item.Title,
                    Quantity = int.Parse(item.QuantityOrdered.ToString()),
                    ShippingCost = item.ShippingPrice == null ? 0.00 : double.Parse(item.ShippingPrice.Amount),
                    SKUItemID =  item.SellerSKU,
                    TransactionPrice = item.ItemPrice == null ? 0.00 : double.Parse(item.ItemPrice.Amount)
                });
            }

            return orderLineItems;



        }
    }
}
