using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infrastrucutre.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System.Collections.Generic;
using MWSClientCsRuntime;
using System.Linq;
using System.Xml;


namespace InventoryManagerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            ListingRepository repository = new ListingRepository();

           var listings = repository.GetListingRequests();


        }

        [TestMethod]
        public void TestMethod9()
        {
            ListingRepository repository = new ListingRepository();
            
            var sellerAccount = repository.GetSeller("arsuk").First();

            List<ItemImage> itemCodes = repository.GetItemCodes(sellerAccount.SellerID);
            List<ItemImage> itemImages = new List<ItemImage>();

            MarketplaceWebServiceProducts.MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProducts.MarketplaceWebServiceProductsConfig();
            config.ServiceURL = "https://mws.amazonservices.co.uk/Products/2011-10-01";
            MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient productsClient = new MarketplaceWebServiceProducts.MarketplaceWebServiceProductsClient("New App", "1.1", sellerAccount.AccessKey, sellerAccount.AuthenticationToken, config);
            MarketplaceWebServiceProducts.Model.GetMatchingProductForIdRequest request = new MarketplaceWebServiceProducts.Model.GetMatchingProductForIdRequest();

            for (int i = 0; i < itemCodes.Count; i+=4)
            {
                request.SellerId = sellerAccount.SellerIdKey;
                request.MarketplaceId = sellerAccount.MarketPlaceId;
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
                        itemImages.Add(new ItemImage() { ImageUrl = imageUrl, ItemID = itemID, SellerID = sellerAccount.SellerID });
                    }

                    repository.InsertImages(itemImages);
                }

            }
            foreach (var itemCode in itemCodes)
            {

            }





        }

        //[TestMethod]
        //public void TestMethod8()
        //{
        //    ListingRepository repository = new ListingRepository();

        //    var sellerAccount = repository.GetSeller("arsuk").First();

        //    IDictionary<string, string> parameters = new SortedDictionary<string, string>(StringComparer.Ordinal);



        //    MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrders.MarketplaceWebServiceOrdersConfig();
        //    config.ServiceURL = "https://mws.amazonservices.co.uk";
        //    MWSClientCsRuntime.MwsConnection mwsConnection = config.CopyConnection();
        //    mwsConnection.AwsAccessKeyId = sellerAccount.AccessKey;
        //    mwsConnection.AwsSecretKeyId = sellerAccount.AuthenticationToken;
        //    mwsConnection.ApplicationName = "New App";
        //    mwsConnection.ApplicationVersion = "1.1";

        //    parameters.Add("AWSAccessKeyId", mwsConnection.AwsAccessKeyId);
        //    parameters.Add("Action", "GetMatchingProduct");

        //    parameters.Add("SellerId", sellerAccount.SellerIdKey);
        //    //parameters.Add("SignatureVersion", "2"); 
        //    //parameters.Add("SignatureMethod", "HmacSHA256");
        //    parameters.Add("MarketplaceId", sellerAccount.MarketPlaceId);
        //    parameters.Add("ASINList.ASIN.1", "B007PP973E");
        //    parameters.Add("Timestamp", MwsUtil.GetFormattedTimestamp());

            

 

        //    string signature = MwsUtil.SignParameters(mwsConnection.GetServiceEndpoint("/Products/2011-10-01").URI, "2", "HmacSHA256", parameters, sellerAccount.AuthenticationToken);


        //    RestClient restClient = new RestClient("https://mws.amazonservices.co.uk");

        //    //&Timestamp=2014-11-12T08%3A20%3A42Z
        //    //&Version=2011-10-01
        //    //&Signature=2vRkDd%2BSD9LWBPAHqEFxjLnW4jzcnVX9%2BFr5QLur1Aw%3D
        //    //&SignatureMethod=HmacSHA256
        //    //&MarketplaceId=A1F83G8C2ARO7P
        //    //&ASINList.ASIN.1=B007PP973E
        //    var request = new RestRequest("Products/2011-10-01", Method.GET);



            

        //    request.AddQueryParameter("AWSAccessKeyId", sellerAccount.AccessKey);
        //    request.AddQueryParameter("Action", "GetMatchingProduct");
        //    request.AddQueryParameter("SellerId", sellerAccount.SellerIdKey);
        //    request.AddQueryParameter("SignatureVersion", "2");
        //    request.AddQueryParameter("Signature", Uri.EscapeDataString(signature));
        //    request.AddQueryParameter("SignatureMethod", "HmacSHA256");
        //    request.AddQueryParameter("MarketplaceId", sellerAccount.MarketPlaceId);
        //    request.AddQueryParameter("ASINList.ASIN.1", "B007PP973E");
        //    request.AddQueryParameter("Timestamp", MwsUtil.GetFormattedTimestamp());

        //   // request.RequestFormat = DataFormat.Json;


        //    var response = restClient.Execute(request);

       
        //}


        [TestMethod]
        public void TestMethod2()
        {
            ListingRepository repository = new ListingRepository();

            var listings = repository.GetListingRequestByID(11);


        }

        [TestMethod]
        public void TestMethod3()
        {
            ListingRepository repository = new ListingRepository();

            var listings = repository.GetUnAllocatedListingRequestNo();

        }

        [TestMethod]
        public void TestMethod4()
        {
            ListingRepository repository = new ListingRepository();

            var orders = repository.GetListingOrders();

        }

        [TestMethod]
        public void TestMethod5()
        {
            ListingRepository repository = new ListingRepository();

            List<Order> orders = new List<Order>();

            List<OrderItem> orderItems = new List<OrderItem>();


            OrderItem l = new OrderItem() { ItemID = "asdasdasd", OrderID = 100, ItemTitle = "asdasdasd", Quantity = 123213, ShippingCost = 12123, TransactionPrice = 13123};

            orderItems.Add(l);

            

            orders.Add(new Order() { AmountPaid=40.00, BuyerUserID="asdasd", CityName="asddsd", Phone="asdasd", Name="asdsadasd", PostalCode="asdasdasd", StateOrProvince="asdasd", Street1="asdasd", Street2="asdasd", OrderItems=  orderItems,OrderReferenceNo="109777626",OrderDate=DateTime.Now,OrderStatus="InProgress", CheckoutState="Complete" });





            var listings = repository.InsertOrders(orders);



        }

        [TestMethod]
        public void TestMethod6() 
        {
            var ac = new SellerAccount()
            {
                SellerID = "account1",
                ChannelName ="Amazon",
                ListingChannelID = 2
            };

            var repository = new ListingRepository();

            repository.InsertSellerAccount(ac);



        }


    }
}
