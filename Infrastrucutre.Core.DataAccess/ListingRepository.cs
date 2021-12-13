using Infrastructure.ConfigurationProvider;
using Infrastructure.Core.DataAccess;
using Infrastrucutre.Core.Models;
using Dapper;
using DapperExtensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DapperExtensions.Mapper;

namespace Infrastrucutre.Core.DataAccess
{
    public static class ListingQueries
    {
        public static string ListingLinkQueryCondition = @"";

        public static string ListingLinkQuery = @"SELECT * FROM (Select ItemMasterID, ItemCode, ItemName, SA.SellerID, SA.ListingChannelID, 
                                        CH.ListingChannelName, ItemLinkId, ListingItemNo, SKU,LinkUrl,
                                        ModifiedByUser, ModifiedDate, SA.SellerIndex,1 as List 
                                        FROM SellerLinkView LinkView ,(SELECT SellerID, ListingChannelID, SellerIndex from SellerAccounts WHERE SellerIndex=@id) SA
                                        INNER JOIN ListingChannel CH on CH.ListingChannelID = SA.ListingChannelID
                                        WHERE (LinkView.SellerIndex=@id)
                                        UNION
                                        Select ItemMasterID, ItemCode, ItemName, SA.SellerID, SA.ListingChannelID, 
                                        CH.ListingChannelName, ItemLinkId, ListingItemNo, SKU,LinkUrl,
                                        ModifiedByUser, ModifiedDate, SA.SellerIndex,CASE WHEN (SELECT ISNULL(COUNT(*),0) FROM SellerItemLink WHERE SellerIndex = SA.SellerIndex and ItemMasterID = LinkView.ItemMasterID) >0 THEN 0 ELSE 1 END
                                        FROM SellerLinkView LinkView ,(SELECT SellerID, ListingChannelID, SellerIndex from SellerAccounts WHERE SellerIndex=@id) SA
                                        INNER JOIN ListingChannel CH on CH.ListingChannelID = SA.ListingChannelID
                                        WHERE (LinkView.SellerIndex is NULL)) AS A WHERE A.List=1";
    }


    public class ListingRepository : IListingRepository
    {
        public List<SellerItemLink> GetItemSellerLink(int id)
        {
            var links = new List<SellerItemLink>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = "Select * from SellerItemLink WHERE ItemMasterID = @id";

                links = connection.Query<SellerItemLink>(query, new { id }).ToList();
            }

            return links;
        }

        
        public List<SellerItemLink> GetListingLinkSubmission(int id)
        {
            var listing = new List<SellerItemLink>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                listing = connection.Query<SellerItemLink>(ListingQueries.ListingLinkQuery, new { id }).ToList();

                return listing;
            }
        }

        public List<SellerItemLink> GetSellerItemLink(out int rowCount, int id, string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0)
        {
            var parameters = new DynamicParameters();

            string whereClause = string.Empty;

            if (filter != "")
            {
                if (filter == "ItemName")
                {
                    whereClause += "ItemName LIKE '%' + @filterText + '%' AND";
                    parameters.Add("@ItemName", filterText.Trim(), DbType.String, ParameterDirection.Input);
                }

                if (filter == "SKU")
                {
                    whereClause += "SKU LIKE '%' + @filterText + '%' AND";
                    parameters.Add("@sku", filterText.Trim(), DbType.String, ParameterDirection.Input);
                }

                if (filter == "ASIN")
                {
                    whereClause += "ListingItemNo LIKE '%' + @filterText + '%' AND";
                    parameters.Add("@itemno", filterText.Trim(), DbType.String, ParameterDirection.Input);
                }

                if (filter == "ItemCode")
                {
                    whereClause += "ItemCode LIKE '%' + @filterText + '%' AND";
                    parameters.Add("@ItemCode", filterText.Trim(), DbType.String, ParameterDirection.Input);
                }

                if (filter == "NotLinked")
                {
                    whereClause += "ItemLinkId IS NULL AND";
                }
            }

            var list = new List<SellerItemLink>();

            rowCount = 0;

            string countquery = string.Format("SELECT ISNULL(SUM(A.CTR),0) AS CTR FROM (Select (CASE WHEN Count(*) = 2 THEN 1 ELSE 1 END) CTR from SellerLinkView WHERE {0} (SellerIndex=@id OR SellerIndex IS NULL) GROUP BY ItemMasterID) A", whereClause);

            string query = string.Format(@"SELECT * FROM (Select ItemMasterID, ItemCode, ItemName, SA.SellerID, SA.ListingChannelID, 
                                        CH.ListingChannelName, ItemLinkId, ListingItemNo, SKU,LinkUrl,
                                        ModifiedByUser, ModifiedDate, SA.SellerIndex,1 as List 
                                        FROM SellerLinkView LinkView ,(SELECT SellerID, ListingChannelID, SellerIndex from SellerAccounts WHERE SellerIndex=@id) SA
                                        INNER JOIN ListingChannel CH on CH.ListingChannelID = SA.ListingChannelID
                                        WHERE {2} (LinkView.SellerIndex=@id)
                                        UNION
                                        Select ItemMasterID, ItemCode, ItemName, SA.SellerID, SA.ListingChannelID, 
                                        CH.ListingChannelName, ItemLinkId, ListingItemNo, SKU,LinkUrl,
                                        ModifiedByUser, ModifiedDate, SA.SellerIndex,CASE WHEN (SELECT ISNULL(COUNT(*),0) FROM SellerItemLink WHERE SellerIndex = SA.SellerIndex and ItemMasterID = LinkView.ItemMasterID) >0 THEN 0 ELSE 1 END
                                        FROM SellerLinkView LinkView ,(SELECT SellerID, ListingChannelID, SellerIndex from SellerAccounts WHERE SellerIndex=@id) SA
                                        INNER JOIN ListingChannel CH on CH.ListingChannelID = SA.ListingChannelID
                                        WHERE {2} (LinkView.SellerIndex is NULL)) AS A WHERE A.List=1 
                                        ORDER BY A.ItemMasterID DESC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;"
                                        , jtStartIndex, jtPageSize, whereClause, ListingQueries.ListingLinkQueryCondition);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                rowCount = connection.Query<int>(countquery, new { id, filterText = filterText.Trim() }).FirstOrDefault();

                var result = connection.Query<SellerItemLink>(query, new { id, filterText = filterText.Trim() });

                return result.ToList();
            }
        }

        public bool UpdateLink(SellerItemLink link)
        {
            int rows = 0;

            link.SKU = link.SKU.Trim();
            link.LinkUrl = link.LinkUrl.Trim();
            link.ListingItemNo = link.ListingItemNo.Trim();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                string query = "SELECT Count(*) FROM SellerItemLink WHERE SellerIndex = @index and ItemMasterID=@itemid and SKU=@sku";

                string updateQuery = @"UPDATE SellerItemLink SET ListingItemNo=@itemno,SKU=@sku,FNSKU =@FNSKU,Postage =@Postage,ActQty=@ActQty,LinkUrl=@url,ModifiedByUser=@userId,ModifiedDate=@date
                                   WHERE  ItemLinkId=@linkid";

                rows = connection.Execute(updateQuery, new
                {
                    sku = link.SKU.Trim(),
                    itemno = link.ListingItemNo.Trim(),
                    FNSKU = link.FNSKU.Trim(),
                    Postage = link.Postage.Trim(),
                    ActQty = link.ActQty.Trim(),
                    url = link.LinkUrl,
                    userId = link.ModifiedByUser,
                    date = DateTime.Now,
                    index = link.SellerIndex,
                    linkid = link.ItemLinkId
                });
            }

            return true;
        }

        public bool DeleteLink(int ItemLinkId) //Delete Sku in Item Master 26-02-2021
        {
            string deleteQuery = string.Format("DELETE FROM SellerItemLink WHERE  ItemLinkId={0}",ItemLinkId);
            

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                IDbTransaction transaction = connection.BeginTransaction();
                int rowsaffected = connection.Execute(deleteQuery, commandType: CommandType.Text, transaction: transaction);
                transaction.Commit();
                return rowsaffected > 0;
            }

        }


        public SellerItemLink InsertAndFix(SellerItemLink link)
        {
            var inserted = new SellerItemLink();

            link.SKU = link.SKU.Trim();
            link.LinkUrl = link.LinkUrl.Trim();
            link.ListingItemNo = link.ListingItemNo.Trim();
            //link.FNSKU = link.FNSKU.Trim();
            //link.Postage = link.Postage.Trim();

            string updateQuery = "UPDATE ItemStock SET ItemMasterID=@id,FNSKU = @FNSKU,Postage = @Postage, ActQty = @ActQty WHERE ListingItemNo=@itemno and SKU=@sku";

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var id = connection.Insert<SellerItemLink>(link);

                connection.Execute(updateQuery, new { id = link.ItemMasterID, itemno = link.ListingItemNo, sku = link.SKU, FNSKU = link.FNSKU, Postage = link.Postage, ActQty = link.ActQty });

                inserted = connection.Get<SellerItemLink>((int)id);
            }

            return inserted;

        }


        public SellerItemLink InsertLink(SellerItemLink link)
        {
            var inserted = new SellerItemLink();

            link.SKU = link.SKU.Trim();
            link.LinkUrl = link.LinkUrl.Trim();
            link.ListingItemNo = link.ListingItemNo.Trim();
            link.FNSKU = link.FNSKU.Trim(); //Add new Field 
            link.Postage = link.Postage.Trim(); // Add new Field 06-02-2021
            link.ActQty = link.ActQty.Trim(); // Add new Field 06-02-2021

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var id = connection.Insert<SellerItemLink>(link);

                inserted = connection.Get<SellerItemLink>((int)id);
            }

            return inserted;
        }

        public SellerItemLink UpdateSellerItemLink(SellerItemLink link)
        {
            string query = "SELECT Count(*) FROM SellerItemLink WHERE SellerIndex = @index and ItemMasterID=@itemid and SKU=@sku";

            string updateQuery = @"UPDATE SellerItemLink SET ListingItemNo=@itemno,SKU=@sku,FNSKU=@FNSKU,Postage=@Postage,ActQty=@ActQty,LinkUrl=@url,ModifiedByUser=@userId,ModifiedDate=@date
                                   WHERE  SellerIndex = @index and ItemMasterID=@itemid and SKU=@sku";

            string insertQuery = @"INSERT INTO SellerItemLink(ItemMasterID, ListingItemNo,SKU,FNSKU,Postage,ActQty,LinkUrl,ModifiedByUser,ModifiedDate,SellerIndex)       
                                  SELECT @itemid,@itemno,@sku,@FNSKU,@Postage,@ActQty,@url,@userId,@date,@index";

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                int? count = connection.Query<int>(query, new { index = link.SellerIndex, itemid = link.ItemMasterID, sku = link.SKU, FNSKU = link.FNSKU, Postage = link.Postage }).FirstOrDefault();

                if (count.HasValue && count > 0)
                {
                    connection.Execute(updateQuery, new
                    {
                        sku = link.SKU,
                        itemno = link.ListingItemNo,
                        FNSKU = link.FNSKU,
                        Postage = link.Postage,
                        ActQty = link.ActQty,
                        url = link.LinkUrl,
                        userId = link.ModifiedByUser,
                        date = DateTime.Now,
                        index = link.SellerIndex,
                        itemid = link.ItemMasterID
                    });
                }
                else
                {
                    connection.Execute(insertQuery, new
                    {
                        sku = link.SKU,
                        itemno = link.ListingItemNo,
                        FNSKU = link.FNSKU,
                        Postage = link.Postage,
                        url = link.LinkUrl,
                        userId = link.ModifiedByUser,
                        date = DateTime.Now,
                        index = link.SellerIndex,
                        itemid = link.ItemMasterID
                    });
                }

            }

            return new SellerItemLink();
        }
        public List<ListingRequest> GetListingRequests(int requestID = 0)
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Listing_GetListings";
                //IEnumerable<ListingRequest> items = connection.Query<ListingRequest>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                parameters.Add("@RequestID", requestID, DbType.Int32, ParameterDirection.Input);


                IEnumerable<ListingRequest> items = connection.Query<ListingRequest, ItemMaster, ListingRequest>(storedProcedure,
                    (request, item) =>
                    {
                        request.Item = item;
                        return request;
                    }
                    , parameters, commandType: CommandType.StoredProcedure,
                    splitOn: "ItemMasterID"
                    );



                return items.ToList();
            }
        }




        public bool Uploadfile(string fileName, string filePath, int requestID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FileName", fileName, DbType.String, ParameterDirection.Input);
            parameters.Add("@path", filePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestID", requestID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "Inventory_UploadDocumentFile";
                IDbTransaction transaction = connection.BeginTransaction();
                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
                return rowsaffected > 0;
            }
        }


        public bool DeleteFile(int requestID, int fileID)
        {
            string deleteQuery = string.Format("Delete from ListingDocuments where RequestID={0} and FileID={1}", requestID, fileID);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                IDbTransaction transaction = connection.BeginTransaction();
                int rowsaffected = connection.Execute(deleteQuery, commandType: CommandType.Text, transaction: transaction);
                transaction.Commit();
                return rowsaffected > 0;
            }


        }


        public List<Submission> GetListingSubmissions(int itemMasterID = 0)
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Listing_GetListingSubmissions";
                //IEnumerable<ListingRequest> items = connection.Query<ListingRequest>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                if (itemMasterID > 0)
                    parameters.Add("@itemMasterID", itemMasterID, DbType.Int32, ParameterDirection.Input);

                IEnumerable<Submission> items = connection.Query<Submission>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                return items.ToList();
            }
        }

        public List<ListingStatus> GetListingStatus()
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = "Select StatusCode,StatusDescription from ListingStatus";

                IEnumerable<ListingStatus> items = connection.Query<ListingStatus>(query, parameters, commandType: CommandType.Text);
                return items.ToList();
            }

        }


        public List<ListingDocument> GetListingRequestImages(int requestID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RequestID", requestID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.GetRequestImages";

                IEnumerable<ListingDocument> listingImages = connection.Query<ListingDocument>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return listingImages.ToList();
            }
        }





        public List<ListingChannel> GetListingChannels()
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = "Select ListingChannelID,ListingChannelName from ListingChannel";

                IEnumerable<ListingChannel> items = connection.Query<ListingChannel>(query, parameters, commandType: CommandType.Text);
                return items.ToList();
            }

        }

        public List<ListingRequestFilter> GetUnAllocatedListingRequestNo()
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                //Select R.ListingRequestNo + '-' + I.ItemName From ListingRequests R 
                //left join ListingSubmissions L ON R.RequestID = l.ListingRequestID
                //inner join ItemMasters I on R.ItemMasterID = I.ItemMasterID
                //where L.ListingSubmissionID IS NULL

                string query = "Select R.ListingRequestNo + '-' + I.ItemName as ListingRequestNo,R.RequestID From ListingRequests R ";
                query += "left join ListingSubmissions L ON R.RequestID = l.ListingRequestID ";
                query += "inner join ItemMasters I on R.ItemMasterID = I.ItemMasterID ";
                query += "where L.ListingSubmissionID IS NULL ";

                IEnumerable<ListingRequestFilter> items = connection.Query<ListingRequestFilter>(query, parameters, commandType: CommandType.Text);
                return items.ToList();
            }


        }

        public bool UpdateCarrier(int PostalCarrierID, DateTime ProceedTime, int[] OrderIDs)
        {
            string updateQuery = string.Format("UPDATE Orders SET PostalCarrierID={0}, ProceedDate='{1}' WHERE OrderID IN({2})", PostalCarrierID, ProceedTime, String.Join(",", OrderIDs));

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                return connection.Execute(updateQuery) > 0;
            }

        }
        public bool updateReactiveDeletedOrders(DateTime ProceedTime, int[] OrderIDs)
        {
            string updateQuery = string.Format("UPDATE Orders SET IsActive=1 WHERE OrderID IN({0})", String.Join(",", OrderIDs));

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                return connection.Execute(updateQuery) > 0;
            }

        }

        public bool UpdateCarrierRef(string CarrierRef, DateTime ProceedTime, int[] OrderIDs)
        {
            //string updateQuery = string.Format("UPDATE OrderItems SET CarrierRef='{0}' WHERE OrderID IN({1})", CarrierRef, String.Join(",", OrderIDs));
            string updateQuery = string.Format("UPDATE Orders SET CarrierRef='{0}' WHERE OrderID IN({1})", CarrierRef, String.Join(",", OrderIDs));

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                return connection.Execute(updateQuery) > 0;
            }

        }
        public bool UpdateAdditionalNotes(string addiotionalNotes, int[] OrderIDs)
        {
            string updateQuery = string.Format("UPDATE Orders SET AddiotionalNotes='{0}' WHERE OrderID IN({1})", addiotionalNotes, String.Join(",", OrderIDs));

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                return connection.Execute(updateQuery) > 0;
            }

        }

        public bool UpdateOrderCarriers(List<Order> orders)
        {

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                IDbTransaction transaction = connection.BeginTransaction();

                foreach (var item in orders)
                {
                    if (item.PostalCarrierID > 0)
                    {
                        var dt = DateTime.Now;
                        connection.Execute("UPDATE Orders SET PostalCarrierID=@PostalCarrierID, ProceedDate=@dt, CarrierRef=@CarrierRef  WHERE OrderID=@OrderID", new { item.PostalCarrierID, dt, item.CarrierRef, item.OrderID }, transaction);
                    }
                    else
                    {

                        connection.Execute("UPDATE Orders SET PostalCarrierID=@PostalCarrierID, CarrierRef=@CarrierRef  WHERE OrderID=@OrderID", new { item.PostalCarrierID, item.CarrierRef, item.OrderID }, transaction);
                    }
                }

                transaction.Commit();

                return true;
            }

        }

        public bool UpdateOrders(List<OrderItem> orderItems)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                foreach (var item in orderItems)
                {
                    connection.Execute(string.Format("UPDATE OrderItems SET AdditionalInfo='{0}' WHERE OrderLineItemID={1}", item.AdditionalInfo/*, item.CarrierRef*/, item.OrderLineItemID)); /*, CarrierRef = '{1}'*/
                }
            }

            return true;
        }


        public List<Order> GetListingOrders(int[] orderIDs = null, string startDate = null, string endDate = null, string sellerId = null, string dispatchStatus = null, string orderNumber = null, bool? specialDelivery = null, string jtSorting = null)
        {

            bool sortyByImages = false;
            //OI.[CarrierRef],
            string query = @"Select [OrderID],OI.[ItemID],OI.[OrderLineItemID],OI.[AdditionalInfo],[ItemTitle],OI.[SKUItemID],[Quantity],
                            [ShippingCost],[TransactionPrice],Im.ImageUrl ";
            query += ",(SELECT S.ChannelName FROM Orders O INNER JOIN SellerAccounts S ON S.SellerID=O.SellerID WHERE O.OrderID = OI.OrderID ) as ChannelName ";
            query += "from OrderItems OI ";
            query += "left join ItemImages Im on OI.ItemID = Im.ItemID ";
            // query += "left join ItemMasters Ims on Ims.ItemMasterID = R.ItemMasterID ";



            string ordersQuery = "Select Orders.*,PostalCarriers.PostalCarrierName,PostalCarriers.Weight,PostalCarriers.Price,PostalCarriers.PostalCarrierID,ISNULL(PostalCarriers.CarrierImage,'NA') as CarrierImage  from Orders left join PostalCarriers ON Orders.PostalCarrierID=PostalCarriers.PostalCarrierID  ";

            if (specialDelivery == true && specialDelivery.HasValue)
                ordersQuery += "inner join OrderItems OI ON Orders.OrderID = OI.OrderID AND ShippingCost>0";

            ordersQuery += " {WHERECLAUSE}";



            if (orderIDs != null && orderIDs.Length > 0)
            {
                string selectedOrders = string.Empty;
                String.Join(",", orderIDs);
                foreach (var item in orderIDs)
                {
                    selectedOrders += item + ",";
                }

                //OrderReferenceNo

                //query += string.Format(" AND OI.OrderID IN(" + String.Join(",", orderIDs) + ")");
                ordersQuery += string.Format(" OrderID IN(" + String.Join(",", orderIDs) + ")");
            }

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                ordersQuery += string.Format(" CAST(OrderDate AS Date)>='" + startDate + "'");
            }


            if (!string.IsNullOrWhiteSpace(endDate))
            {
                ordersQuery += string.Format(" AND Cast(OrderDate AS Date)<='" + endDate + "'");
            }

            if (!string.IsNullOrWhiteSpace(sellerId))
            {
                ordersQuery += string.Format(" AND SellerIndex=" + sellerId + " ");
            }

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                ordersQuery += string.Format(" AND SalesOrderNumber='" + orderNumber + "'");
            }

            if (string.IsNullOrWhiteSpace(dispatchStatus))
            {
                //ordersQuery += string.Format(" AND SellerID='" + sellerId + "'");
            }
            else if (dispatchStatus == "Pending")
            {
                ordersQuery += string.Format(" AND ShippedDate IS NULL");
            }
            else if (dispatchStatus == "Delete")
            {
                ordersQuery += string.Format(" AND IsActive=0");
            }
            else
            {
                ordersQuery += string.Format(" AND ShippedDate IS NOT NULL");
            }

            if (dispatchStatus != "Delete")
            {
                ordersQuery += string.Format(" AND IsActive=1");
            }


            if (string.IsNullOrWhiteSpace(jtSorting))
            {
                ordersQuery += " Order By OrderDate Desc";
            }
            else
            {
                if (jtSorting.Contains("PostalCarrierName"))
                    jtSorting = jtSorting.Replace("PostalCarrierName", "Orders.PostalCarrierID");

                if (jtSorting.Contains("Images"))
                {
                    sortyByImages = true;
                    jtSorting = jtSorting.Replace("Images", "OrderDate");
                }

                ordersQuery += " Order By " + jtSorting;
            }

            ordersQuery = ordersQuery.Replace("{WHERECLAUSE}", " Where OrderStatus IN('Completed','Shipped','Unshipped') AND ");


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                IEnumerable<Order> orders = connection.Query<Order>(ordersQuery);
                List<int> lstOrderId = orders.Select(x => x.OrderID).ToList();
                //var test = string.Format(" AND OI.OrderID IN('" + string.Join(",", lstOrderId) + "')");
                query += string.Format(" where OI.OrderID IN('" + string.Join("','", lstOrderId) + "')");
                IEnumerable<OrderItem> orderItems = new List<OrderItem>();
                IEnumerable<StockView> stockViews = new List<StockView>();
                if (lstOrderId.Count() > 0)
                {
                    orderItems = connection.Query<OrderItem>(query, commandType: CommandType.Text);
                }


                List<string> lstListingItemId = orderItems.Select(x => x.ItemID).Distinct().ToList();
                //var queryForItemStock = "select * from ItemStock where ListingItemNo in('" + string.Join("','", lstListingItemId) + "')";
                //IEnumerable<ItemStock> itemStocks = connection.Query<ItemStock>(queryForItemStock);
                foreach (var order in orders)
                {
                    order.OrderItems = orderItems.Where(t => t.OrderID == order.OrderID).ToList();
                    foreach (var orderItem in order.OrderItems)
                    {
                        try
                        {
                            var sellerItemLinkQuery = "select SellerItemLink.*, im.ItemName,im.ItemCode from SellerItemLink " +
                            " left join ItemMasters Im on SellerItemLink.ItemMasterID = Im.ItemMasterID" +
                            " where SellerItemLink.SKU like '%" + orderItem.SKUItemID.Trim('\'') + "%'";

                            //"select * from SellerItemLink where SKU like '%" + orderItem.SKUItemID.Trim('\'') + "%'";
                            SellerItemLink sellerItemLinks = connection.Query<SellerItemLink>(sellerItemLinkQuery).FirstOrDefault();
                            if (sellerItemLinks != null)
                            {
                                var stockViewLinkQuery = "select * from StockView where ItemMasterId=" + sellerItemLinks.ItemMasterID + "";
                                StockView stockView = connection.Query<StockView>(stockViewLinkQuery).FirstOrDefault();
                                orderItem.StockUnits = stockView.CurrentStock;
                                orderItem.ItemName = sellerItemLinks.ItemName;
                                orderItem.ItemCode = sellerItemLinks.ItemCode;
                                //orderItem.StockUnits = itemStocks.Where(x => x.ListingItemNo == orderItem.ItemID).FirstOrDefault() != null ? itemStocks.Where(x => x.ListingItemNo == orderItem.ItemID).FirstOrDefault().InSource : 0;

                            }

                            // var sellerItemLinkQuery = "select SellerItemLink.*, im.ItemName,im.ItemCode from SellerItemLink " +
                            //"left join ItemMasters Im on SellerItemLink.ItemMasterID = Im.ItemMasterID" +
                            //"where SKU like '%" + orderItem.SKUItemID.Trim('\'') + "%'";
                            // SellerItemLink sellerItemLinks = connection.Query<SellerItemLink>(sellerItemLinkQuery).FirstOrDefault();
                            // if (sellerItemLinks != null)
                            // {

                            //     //query += "left join ItemMaster Itm on OI.ItemID = Im.ItemID ";

                            //     var stockViewLinkQuery = "select * from StockView where ItemMasterId=" + sellerItemLinks.ItemMasterID + "";
                            //     StockView stockView = connection.Query<StockView>(stockViewLinkQuery).FirstOrDefault();
                            //     orderItem.StockUnits = stockView.CurrentStock;
                            //     orderItem.ItemName = sellerItemLinks.ItemName;
                            //     orderItem.ItemCode = sellerItemLinks.ItemCode;
                            //     //orderItem.StockUnits = itemStocks.Where(x => x.ListingItemNo == orderItem.ItemID).FirstOrDefault() != null ? itemStocks.Where(x => x.ListingItemNo == orderItem.ItemID).FirstOrDefault().InSource : 0;

                            // }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    //order.FirstItemID = order.OrderItems.First().ItemID;
                }


                //var distinct = orderItems.Select(m => new { m.ItemID, m.ItemTitle }).Distinct().ToList();


                if (sortyByImages)
                {
                    orders = orders.OrderByDescending(o => o.OrderItems.First().ItemID);
                }

                return orders.ToList();

            }

        }



        public ListingRequest GetListingRequestByID(int requestID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                ListingRequest request = connection.Get<ListingRequest>(requestID);

                return request;
            }
        }

        //_________________________Edit Fba Request Details ________________________________//

        public FbaRequest GetFbaRequestByID(int FBARequestID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                FbaRequest FBArequest = connection.Get<FbaRequest>(FBARequestID);

                return FBArequest;
            }
        }

        public bool UpdateFbaRequest(FbaRequest request)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<FbaRequest>(request);
            }

            return updateCompleted;
        }

        
        public List<FbaRequest> GetFbaPendingRequest(int jtStartIndex = 0, int jtPageSize = 0,int SellerIndex = 0, int FBARootID = 0) //Asin Report 
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<FbaRequest>("FBAPendingRequest", new {jtStartIndex= jtStartIndex, jtPageSize = jtPageSize, SellerIndex = SellerIndex, FBARootID = FBARootID }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        //_______________FBA SORTED LIST _________________________//
        public List<FbaRequest> GetFbaSortedList(int jtStartIndex = 0, int jtPageSize = 0, int SellerIndex = 0, int FBARootID = 0) //Asin Report 
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<FbaRequest>("FBASortedList", new { jtStartIndex = jtStartIndex, jtPageSize = jtPageSize, SellerIndex = SellerIndex, FBARootID = FBARootID }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public bool UpdateFbaProcesingByID(int FBARequestID) // Update Process Status will 'Accept / Active'
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("UPDATE FbaRequest SET Status='ACTIVE' WHERE FBARequestID = {0}", FBARequestID);
                int rowsAffected = connection.Execute(query, commandType: CommandType.Text);
                return rowsAffected > 0;
            }

        }

        public bool RejectFbaProcesingByID(int FBARequestID) // REJECT Process Status will 'Pending'
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("UPDATE FbaRequest SET Status='Pending' WHERE FBARequestID = {0}", FBARequestID);
                int rowsAffected = connection.Execute(query, commandType: CommandType.Text);
                return rowsAffected > 0;
            }

        }

        public bool AddShipment(string ShipmentID,string ShipmentMethod,string Destination,  DateTime ProceedTime,decimal[] FinalQty, decimal[] FBABoxQty, int[] OrderIDs)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                IDbTransaction transaction = connection.BeginTransaction();
                var parameters = new DynamicParameters();

                StringBuilder accessListXml = new StringBuilder();

                int i = 0;
                foreach (var item in OrderIDs)
                {
                    
                            parameters.Add("@ShipmentID", ShipmentID, DbType.String, ParameterDirection.Input);
                        parameters.Add("@FBARequestID", item, DbType.String, ParameterDirection.Input);
                        parameters.Add("@ShipmentMethod", ShipmentMethod, DbType.String, ParameterDirection.Input);
                        parameters.Add("@Destination", Destination, DbType.String, ParameterDirection.Input);
                        parameters.Add("@FinalQty", FinalQty[i], DbType.String, ParameterDirection.Input);
                        parameters.Add("@FBABoxQty", FBABoxQty[i], DbType.String, ParameterDirection.Input);
                        parameters.Add("@Del", 'N', DbType.String, ParameterDirection.Input);
                        parameters.Add("@AccessList", accessListXml.ToString(), DbType.Xml, ParameterDirection.Input);

                        const string storedProcedure = "dbo.FBA_AddShipment";
                        connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                    i++;
                    
                }
                const string storedProced = "dbo.FBA_UpdateShipment";
                connection.Execute(storedProced, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();

                return true;
                //return rowsaffected > 0;
            }
        }

        //_________________________________________________________________________//

        public List<FbaRequest> GetFbaRequest(out int totalCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            var parameters = new DynamicParameters();

            

            string whereClause = string.Empty;

            string countQuery = string.Format(@"SELECT COUNT(*) FROM FbaRequest WHERE Del = 'N' AND Status = 'Pending'", whereClause);

            string query = string.Format(@"SELECT 
                                           R.FBARequestID,R.ListingItemNo,R.ItemMasterID,R.SKU,R.UserID,R.SellerIndex,R.UKSold30Days,
                                           R.FNSKU,R.LableStatus,R.LableLink,R.LastReqDate,R.PriorityStatus,F.FBARoot,R.Date,R.Comments,
                                           R.FBARecedQty,R.RequestQty,R.Comments,I.ItemMasterID,ItemName,MasterCartonQty,I.CartonQty,
                                           ItemCode,Brand,ItemWeight,CurrentStock As UKWarehouse,UKFbaStock,
                                           (cast(Length as varchar(10)) + 'x' + cast(Width as varchar(10)) + 'x' + cast(Height as varchar(10))) As Dimension1
                                           ,U.UserName,sa.SellerID
  
                                           FROM FbaRequest R inner join 
                                           ItemMasters I ON I.ItemMasterID = R.ItemMasterID
                                           inner join FBARoot F ON R.FBARootID = F.FBARootID
                                           inner join stockview sv ON sv.ItemMasterID = R.ItemMasterID
                                           inner join AppUsers U ON  R.UserID = U.UserID
                                           inner join SellerAccounts sa ON R.SellerIndex = sa.SellerIndex
  
                                           WHERE R.Del = 'N' AND R.Status = 'Pending'{2}
                                           ORDER BY R.FBARequestID DESC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", jtStartIndex, jtPageSize, whereClause);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                var reader = connection.QueryMultiple(countQuery + "\n" + query, parameters);

                totalCount = reader.Read<int>().Single();

                var result = reader.Read<FbaRequest>();

                return result.ToList();
            }
        }

        //_________________________________________________FBA SHIPMENT DETAILS_________________________________//

        public List<ShipmentDetails> GetShipmentDetails(out int totalCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            var parameters = new DynamicParameters();

            string whereClause = string.Empty;
            string countQuery = string.Format(@"SELECT  COUNT(Distinct ShipmentID) FROM Shipment WHERE Del = 'N' ", whereClause);

            string query = string.Format(@"SELECT S.ShipmentID,S.ShipmentMethod,S.Destination,Cast(S.ProceedDate as date) As ProceedDate,
                                                  SA.SellerID,R.FBARoot,S.TotalBoxes,S.TotalFNSKU,S.TotalQty
                                           FROM Shipment S  JOIN FBARequest F ON F.FBARequestID = S.FBARequestID
                                           inner Join SellerAccounts SA ON F.SellerIndex = SA.SellerIndex
                                           Inner JOIN FBARoot R ON F.FBARootID = R.FBARootID
                                           WHERE S.Del = 'N' {2}
                                           GROUP BY S.ShipmentID,S.ShipmentMethod,S.Destination,Cast(S.ProceedDate as date),
                                           SA.SellerID,R.FBARoot,S.TotalBoxes,S.TotalFNSKU,S.TotalQty ORDER BY S.ShipmentID DESC

                                           OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", jtStartIndex, jtPageSize, whereClause);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                var reader = connection.QueryMultiple(countQuery + "\n" + query, parameters);

                totalCount = reader.Read<int>().Single();

                var result = reader.Read<ShipmentDetails>();

                return result.ToList();
            }

            //using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            //{
            //    var result = connection.Query<ShipmentDetails>("FBA_ShipmentDetails", new {  }, commandType: CommandType.StoredProcedure);

            //    return result.ToList();
            //}

        }

        public List<FbaRequest> GetShipmentHistory(string id, out int rowCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            string query = string.Format(@"SELECT  R.FBARequestID,R.ListingItemNo,R.ItemMasterID,R.SKU,R.UserID,R.SellerIndex,R.UKSold30Days,
                                           R.FNSKU,R.LableStatus,R.LableLink,R.LastReqDate,R.PriorityStatus,
                                           R.FBARecedQty,R.RequestQty,R.Comments,I.ItemMasterID,ItemName,MasterCartonQty,I.CartonQty,
                                           ItemCode,Brand,ItemWeight,CurrentStock As UKWarehouse,UKFbaStock,
                                           (cast(Length as varchar(10)) + 'x' + cast(Width as varchar(10)) + 'x' + cast(Height as varchar(10))) As Dimension1
                                           ,U.UserName,F.FBARoot,Cast(R.Date As Date) As Date,s.FinalQty,s.FBABoxQty,s.ProceedDate As SendingDate
   
                                           FROM FbaRequest R 
	                                       inner join ItemMasters I ON I.ItemMasterID = R.ItemMasterID
                                           inner join FBARoot F ON R.FBARootID = F.FBARootID
                                           inner join stockview sv ON sv.ItemMasterID = R.ItemMasterID
                                           inner join AppUsers U ON  R.UserID = U.UserID
                                           inner Join Shipment s On s.FBARequestID = R.FBARequestID
  
                                           WHERE R.Del = 'N' AND Status = 'ACTIVE' AND R.Sorted ='Yes' AND s.ShipmentID = @id
                                           ORDER BY S.ShipmentID DESC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", jtStartIndex, jtPageSize);

            string countQuery = @"SELECT COUNT(*) FROM FbaRequest F Join Shipment s On s.FBARequestID = F.FBARequestID
                                  WHERE F.Del = 'N' AND s.ShipmentID = @id;";
            rowCount = 0;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var reader = connection.QueryMultiple(countQuery + query, new { id = id });

                rowCount = reader.Read<int>().Single();

                var result = reader.Read<FbaRequest>();

                return result.ToList();
            }
        }

        public List<FbaRequest> GetFbaRequestHistory(string id, out int rowCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            string query = string.Format(@"SELECT  R.FBARequestID,R.ListingItemNo,R.ItemMasterID,R.SKU,R.UserID,R.SellerIndex,R.UKSold30Days,
                                           R.FNSKU,R.LableStatus,R.LableLink,R.LastReqDate,R.PriorityStatus,
                                           R.FBARecedQty,R.RequestQty,R.Comments,I.ItemMasterID,ItemName,MasterCartonQty,I.CartonQty,
                                           ItemCode,Brand,ItemWeight,CurrentStock As UKWarehouse,UKFbaStock,
                                           (cast(Length as varchar(10)) + 'x' + cast(Width as varchar(10)) + 'x' + cast(Height as varchar(10))) As Dimension1
                                           ,U.UserName,F.FBARoot,Cast(R.Date As Date) As Date

                                           FROM FbaRequest R 
	                                       inner join ItemMasters I ON I.ItemMasterID = R.ItemMasterID
                                           inner join FBARoot F ON R.FBARootID = F.FBARootID
                                           inner join stockview sv ON sv.ItemMasterID = R.ItemMasterID
                                           inner join AppUsers U ON  R.UserID = U.UserID
                                           
  
                                           WHERE R.Del = 'N' AND Status = 'ACTIVE' AND R.Sorted ='Yes' AND R.ItemMasterID = @id
                                           ORDER BY R.ItemMasterID DESC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", jtStartIndex, jtPageSize);

            string countQuery = @"SELECT COUNT(*) FROM FbaRequest F WHERE F.Del = 'N' AND F.Sorted ='Yes' AND F.ItemMasterID = @id;";
            rowCount = 0;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var reader = connection.QueryMultiple(countQuery + query, new { id = id });

                rowCount = reader.Read<int>().Single();

                var result = reader.Read<FbaRequest>();

                return result.ToList();
            }
        }
        public bool DeleteShipment(string ShipmentID) // REJECT Process Status will 'Pending'
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("DELETE Shipment WHERE ShipmentID = '{0}'", ShipmentID);
                int rowsAffected = connection.Execute(query, commandType: CommandType.Text);
                return rowsAffected > 0;
            }

            //using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            //{
            //    var result = connection.Query<FbaRequest>("FBADeleteShipment", new { ShipmentID = ShipmentID}, commandType: CommandType.StoredProcedure);

            //    return result.ToList();
            //}

        }

        public List<ShipmentDetails> GetTotalFNSKU(string ShipmentID ="")
        {
            string countQuery = string.Format(@"SELECT  COUNT(Distinct ShipmentID) FROM Shipment WHERE Del = 'N' AND ShipmentID = @ShipmentID ");

            string query = string.Format(@"SELECT Count(FNSKU) FROM Shipment S JOIN FBARequest F ON F.FBARequestID = S.FBARequestID 
	                                       WHERE Status = 'ACTIVE' AND Sorted = 'Yes' AND S.ShipmentID = '{0}'",ShipmentID);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                var reader = connection.QueryMultiple(query);

                //countQuery = reader.Read<int>().Single();

                var result = reader.Read<ShipmentDetails>();

                return result.ToList();
            }
        }
        //______________________________________________________________________________________________________//

        public ListingSubmission GetListingSubmissionByID(int submissionID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                ListingSubmission submission = connection.Get<ListingSubmission>(submissionID);

                return submission;
            }

        }

        public bool UpdateSellerAccount(SellerAccount account)
        {
            int updated = 0;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string update = @"UPDATE SellerAccounts SET [ListingChannelID]=@channelid,[ChannelName]=@channelname,[Synchronize]=@sync 
                                  WHERE SellerIndex=@index";

                string channel = connection.Query<string>("SELECT ListingChannelName FROM ListingChannel WHERE ListingChannelID=@id", new { id = account.ListingChannelID }).FirstOrDefault();

                account.ChannelName = channel;

                updated = connection.Execute(update, new
                {
                    channelid = account.ListingChannelID,
                    channelname = account.ChannelName,
                    index = account.SellerIndex,
                    sync = account.Synchronize
                });
            }

            return updated > 0;
        }

        public bool UpdateSellerAuth(SellerAccount account)
        {
            int updated = 0;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string update = @"UPDATE SellerAccounts SET [AuthenticationToken]=@token,[AccessKey]=@accesskey,[MarketplaceId]=@marketid,[SellerIdKey]=@sellerkey
                                  WHERE SellerIndex=@index";

                updated = connection.Execute(update, new
                {
                    token = account.AuthenticationToken,
                    accesskey = account.AccessKey,
                    marketid = account.MarketPlaceId,
                    sellerkey = account.SellerIdKey,
                    index = account.SellerIndex
                });
            }

            return updated > 0;
        }

        public SellerAccount InsertSellerAccount(SellerAccount account)
        {
            var newac = new SellerAccount();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string insert = @"INSERT INTO SellerAccounts(ListingChannelID,ChannelName,SellerID,AuthenticationToken) 
                                Output Inserted.SellerIndex
                                Values(@channelid,@channelname,@sellerid,'N/A')";

                account.AuthenticationToken = "Default";

                string channel = connection.Query<string>("SELECT ListingChannelName FROM ListingChannel WHERE ListingChannelID=@id", new { id = account.ListingChannelID }).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(channel))
                {
                    account.ChannelName = channel;
                }

                var id = connection.Insert<SellerAccount>(account);

                newac = connection.Get<SellerAccount>((int)id);
            }

            return newac;

        }


        public bool SubmitListing(ListingSubmission submission)
        {
            int listingSubmissionID;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                listingSubmissionID = connection.Insert<ListingSubmission>(submission);

                return listingSubmissionID > 0;
            }
        }

        public bool UpdateSubmission(ListingSubmission submission)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<ListingSubmission>(submission);

                return updateCompleted;
            }
        }



        public bool AddListing(ListingRequest request, out int listingRequestID)
        {
            while (true)
            {
                request.ListingRequestNo = GetListingRequestNumber(request.ItemMasterID, string.Empty, true);

                if (!RequestNoExists(request.ListingRequestNo))
                    break;
            }

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                listingRequestID = connection.Insert<ListingRequest>(request);

                return listingRequestID > 0;
            }
        }

        //__________________________________ FBA REQUEST FORM INSERT DATA 04-03-2021 __________________________________//

        
        public bool FBARequest(FbaRequest  request, out int FBARequestID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                FBARequestID = connection.Insert<FbaRequest>(request);

                return FBARequestID > 0;
            }
        }
        //______________________________________________________________________________________________________________________________//
        public bool RequestNoExists(string requestNo)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                IEnumerable<dynamic> values = connection.Query(string.Format("select 1 from ListingRequests where ListingRequestNo='{0}'", requestNo), commandType: CommandType.Text);

                return values.Count() > 0;

            }

        }

        public string GetListingRequestNumber(int itemMasterID, string itemName, bool isForUpdate)
        {

            string lastSequenceNumber = string.Empty;


            var parameters = new DynamicParameters();
            parameters.Add("@ItemMasterID", itemMasterID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@IsForUpdate", isForUpdate, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            parameters.Add("@SequenceNo", dbType: DbType.String, direction: ParameterDirection.Output, size: 200);
            parameters.Add("@ItemName", dbType: DbType.String, direction: ParameterDirection.Output, size: 200);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Listing_GetListingSequence";
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                lastSequenceNumber = parameters.Get<string>("@SequenceNo");
                itemName = parameters.Get<string>("@ItemName");
            }

            return itemName.Split(' ').First() + "_" + lastSequenceNumber;


        }


        public bool UpdateListing(ListingRequest request)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<ListingRequest>(request);
            }

            return updateCompleted;
        }

        public List<OrderState> GetOrderStates()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var orderStates = connection.Query<OrderState>("select distinct IsNull(OrderStatus,'All') as OrderStatus from Orders");

                return orderStates.ToList();

            }
        }

        public List<SellerAccount> GetSellerAccounts()
        {
            var accounts = new List<SellerAccount>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT [SellerID]
                                ,[ListingChannelID]     
                                ,[ChannelName]      
                                ,[SellerIndex]
                                ,[Synchronize]
                            FROM [dbo].[SellerAccounts]";

                accounts = connection.Query<SellerAccount>(query).ToList();
            }

            return accounts;
        }

        public List<AutoCompleteItem> GetSellerList()
        {
            List<AutoCompleteItem> sellers = new List<AutoCompleteItem>();

            string query = @"Select channel.ListingChannelName + '-' + SellerID  as label,SellerIndex as id from SellerAccounts accounts inner join ListingChannel channel on accounts.ListingChannelID = channel.ListingChannelID
                            order by channel.ListingChannelName";

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                sellers = connection.Query<AutoCompleteItem>(query).ToList();
            }

            return sellers;
        }

        public List<AutoCompleteItem> GetPostageList()
        {
            List<AutoCompleteItem> sellers = new List<AutoCompleteItem>();

            string query = @"Select PostalCarrierName as label,PostalCarrierID as id from PostalCarriers
                            order by PostalCarrierName";

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                sellers = connection.Query<AutoCompleteItem>(query).ToList();
            }

            return sellers;
        }
        //------------------------------------------------------------------------------------------------
        public List<ItemMaster> GetItemsList(string ItemMasterID = "") // Get Item List 15-10-2020 # Danish
        {
            var itemsName = new List<ItemMaster>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT [ItemMasterID],[ItemName]     
                                FROM [dbo].[ItemMasters]";

                itemsName = connection.Query<ItemMaster>(query).ToList();
            }

            return itemsName;
        }

        public List<ItemMaster> GetSKUList(string ItemMasterID = "", bool? sync = false) // Get SKU List 24-10-2020 # Danish
        {
            var SKU = new List<ItemMaster>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("SELECT [ItemMasterID],[SKU],[ListingItemNo] FROM[arsukeuro_mssql].[dbo].[SellerItemLink] WHERE  ListingItemNo IS NOT NULL Group By[ItemMasterID],[SKU],[ListingItemNo]  ", ItemMasterID);

                SKU = connection.Query<ItemMaster>(query).ToList();
            }

            return SKU;
        }

        public List<ItemMaster> GetAsinsList(string ItemMasterID = "") // Get Asins List 24-10-2020 # Danish
        {
            var AsinNo = new List<ItemMaster>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT [ItemMasterID],[SKU],[ListingItemNo] 
                                 FROM   [arsukeuro_mssql].[dbo].[SellerItemLink]
                                 WHERE  ListingItemNo IS NOT NULL
								 Group By [ItemMasterID],[SKU],[ListingItemNo]";

                AsinNo = connection.Query<ItemMaster>(query).ToList();
            }

            return AsinNo;
        }

        public List<ItemMaster> GetAsinByID(string ItemMasterID = "") // Get SKU List 24-10-2020 # Danish
        {
            var SKU = new List<ItemMaster>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT [StockId],[ItemMasterID],[StockIn],[SKU],[ListingItemNo] 
                                 FROM   [arsukeuro_mssql].[dbo].[ItemStock]
                                 WHERE  ListingItemNo IS NOT NULL";

                SKU = connection.Query<ItemMaster>(query).ToList();
            }

            return SKU;

        }

        public List<ItemMaster> GetUserItemByID(string ItemMasterID = "") // Get Group User Item List 24-10-2020 # Danish
        {
            var SKU = new List<ItemMaster>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT G.UserID,I.ItemName,I.ItemMasterID
                                 FROM GroupControll G JOIN AppUsers AU ON G.UserID = AU.UserID JOIN ItemMasters I ON G.ItemMasterID = I.ItemMasterID
                                 WHERE G.UserID = @UserID AND G.Del = 'N' ";

                SKU = connection.Query<ItemMaster>(query).ToList();
            }

            return SKU;

        }


        public List<ItemMaster> GetSKUByID(string ListingItemNo = "") // Get SKU List 24-10-2020 # Danish
        {
            var SKU = new List<ItemMaster>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT SKU,ItemMasters.ItemMasterID
                                 FROM [arsukeuro_mssql].[dbo].[ItemStock]
                                 JOIN ItemMasters ON ItemMasters.ItemMasterID = ItemStock.ItemMasterID 
                                 WHERE ListingItemNo IS NOT NULL AND ListingItemNo = @ListingItemNo
                                 Group By SKU,ItemMasters.ItemMasterID";

                SKU = connection.Query<ItemMaster>(query).ToList();
            }

            return SKU;

        }

        public List<ItemMaster> GetItemsListByID(string SellerID = "") // Get SKU List 24-10-2020 # Danish
        {
            var SKU = new List<ItemMaster>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT ItemName,SellerItemLink.ItemMasterID
                                 FROM [arsukeuro_mssql].[dbo].[SellerItemLink] JOIN ItemMasters ON SellerItemLink.ItemMasterID = ItemMasters.ItemMasterID
                                 WHERE ListingItemNo IS NOT NULL AND SellerItemLink.SellerIndex = @SellerID
                                 Group By ItemName,SellerItemLink.ItemMasterID";

                SKU = connection.Query<ItemMaster>(query).ToList();
            }

            return SKU;

        }

        public List<ItemDetails> GetFNSKUList(string ItemMasterID = "") // Get Asins List 24-10-2020 # Danish
        {
            var FNSKU = new List<ItemDetails>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT [SKU],[FNSKU],[ItemMasterID]
                                 FROM   [arsukeuro_mssql].[dbo].[SellerItemLink]
                                  WHERE  [FNSKU] IS NOT NULL Group By [SKU],[FNSKU],[ItemMasterID] ";

                FNSKU = connection.Query<ItemDetails>(query).ToList();
            }

            return FNSKU;
        }

        public List<FBALocations> GetFBARootList(string FBARootID = "", bool? sync = false) // Get Asins List 24-10-2020 # Danish
        {
            var FBARoot = new List<FBALocations>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT FBARootID,FBARoot
                                 FROM   [arsukeuro_mssql].[dbo].[FBARoot]
                                  WHERE  [FBARoot] IS NOT NULL AND Del = 'N'";

                FBARoot = connection.Query<FBALocations>(query).ToList();
            }

            return FBARoot;
        }
        //----------------------------------------------------------------------------------------------------------------------//

        //----------------------- Get Group User For Report List 02-12-2021-----------------------//

            public List<UserInformation> GetGroupUser(string UserID = "", bool? sync = false)
           {
            var Users = new List<UserInformation>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT [GroupControll].[UserID],[UserName] FROM [arsukeuro_mssql].[dbo].[GroupControll]
                                 JOIN [arsukeuro_mssql].[dbo].[AppUsers] ON [AppUsers].UserID = [GroupControll].UserID
                                 Group By [GroupControll].[UserID],[UserName]";

                Users = connection.Query<UserInformation>(query).ToList();
            }

            return Users;
        }

        // ------------------------------ END -----------------------------------------//

        //--------------------------Get User For Group Assign  03-02-2021 --------------------------//

        public List<UserInformation> GetUser(string UserID = "", bool? sync = false)
        {
            var Users = new List<UserInformation>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"SELECT [UserID],[UserName]
                                 FROM [arsukeuro_mssql].[dbo].[AppUsers] Where IsActive = 1";

                Users = connection.Query<UserInformation>(query).ToList();
            }

            return Users;
        }

        public bool AddGroup(int UserID, DateTime ProceedTime, int[] OrderIDs)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                IDbTransaction transaction = connection.BeginTransaction();
                var parameters = new DynamicParameters();

            StringBuilder accessListXml = new StringBuilder();

            foreach (var item in OrderIDs)
            {
                
                parameters.Add("@UserID", UserID, DbType.String, ParameterDirection.Input);
                parameters.Add("@ItemMasterID", item, DbType.String, ParameterDirection.Input);
                //parameters.Add("@ProceedDate", ProceedTime, DbType.String, ParameterDirection.Input);
                parameters.Add("@Del", 'N', DbType.String, ParameterDirection.Input);
                parameters.Add("@AccessList", accessListXml.ToString(), DbType.Xml, ParameterDirection.Input);

               const string storedProcedure = "dbo.Inventory_AddGroup";
               connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
 
            }
                transaction.Commit();

                return true;
                //return rowsaffected > 0;
            }
        }

        //------------------------------------------------------------------------------------------//


        public List<SellerAccount> GetSeller(string sellerID = "", bool? sync = false)
        {

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<SellerAccount> sellers = new List<SellerAccount>();

                if (!string.IsNullOrWhiteSpace(sellerID))
                {
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                    pg.Predicates.Add(Predicates.Field<SellerAccount>(f => f.SellerIndex, Operator.Eq, sellerID));

                    sellers = connection.GetList<SellerAccount>(pg).ToList();
                }
                else
                {
                    var pg = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

                    if (sync.HasValue && sync.Value == true)
                    {
                        pg.Predicates.Add(Predicates.Field<SellerAccount>(f => f.Synchronize, Operator.Eq, sync.Value));
                    }

                    sellers = connection.GetList<SellerAccount>(pg).ToList();
                }

                return sellers;

            }

        }

        public bool CheckExistingOrder(string orderRefNo)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                var predicate = Predicates.Field<Order>(f => f.OrderReferenceNo, Operator.Eq, orderRefNo);
                var existingOrder = connection.GetList<Order>(predicate);
                if (existingOrder.Count() > 0)
                {
                    return true;
                }

            }

            return false;

        }


        public List<ItemImage> GetItemCodes(string sellerID)
        {
            //select distinct oi.ItemID,'',O.SellerID from orders o inner join OrderItems oi on o.OrderID = oi.OrderID
            //WHERE SellerID IS NOT NULL 

            string query = string.Format("SELECT DISTINCT OI.ITEMID FROM ORDERS O INNER JOIN ORDERITEMS OI ON O.ORDERID = OI.ORDERID " +
                                         "LEFT JOIN ITEMIMAGES IM ON IM.ITEMID = OI.ITEMID " +
                                         "WHERE O.SellerID='{0}' AND IM.IMAGEURL IS NULL", sellerID);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var items = connection.Query<ItemImage>(query, commandType: CommandType.Text);

                return items.ToList();
            }
        }


        public bool InsertImages(List<ItemImage> images)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                foreach (var item in images)
                {
                    var predicate = Predicates.Field<ItemImage>(f => f.ItemID, Operator.Eq, item.ItemID);

                    var existingImage = connection.GetList<ItemImage>(predicate);

                    if (existingImage.Count() == 0)
                    {
                        int itemID = connection.Insert<ItemImage>(item);
                    }
                    else
                    {
                        ItemImage updatedImage = existingImage.First();
                        updatedImage.ImageUrl = item.ImageUrl;
                        connection.Update<ItemImage>(updatedImage);
                    }
                }

                updateCompleted = true;
            }

            return updateCompleted;
        }


        public bool InsertSalesCommission(SaleOrder sale, out string message)
        {

            StringBuilder salesOrderString = new StringBuilder();

            salesOrderString.Append("INSERT INTO SalesOrder(OrderID,ShipmentID,MarketplaceName) Values(@OrderID,@ShipmentID,@MarketplaceName);SELECT CAST(@@IDENTITY AS INT)");


            StringBuilder salesOrderItemString = new StringBuilder();

            salesOrderItemString.Append("INSERT INTO SalesOrderItems(OrderItemCode,SKU,Quantity,SalesOrderID) Values(@ItemCode,@SKU,@Quantity,@salesOrderId);SELECT CAST(@@IDENTITY AS INT)");


            StringBuilder salesCommissionString = new StringBuilder();

            salesCommissionString.Append("INSERT INTO SalesCommission(OrderReferenceNo,OrderItemCode,FeesType,Amount,SalesItemId) Values(@OrderID,@ItemCode,@Type,@Amount,@salesItemId) ");

            StringBuilder salesPriceString = new StringBuilder();

            salesPriceString.Append("INSERT INTO SalesPrice(OrderReferenceNo,OrderItemCode,FeesType,Amount,SalesItemId) Values(@OrderID,@ItemCode,@Type,@Amount,@salesItemId) ");


            message = string.Empty;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                int salesOrderId = connection.Query<int>(salesOrderString.ToString(), new { sale.OrderID, sale.ShipmentID, sale.MarketplaceName }).SingleOrDefault();

                foreach (var item in sale.OrderItems)
                {
                    int salesItemId = connection.Query<int>(salesOrderItemString.ToString(), new { item.ItemCode, item.SKU, item.Quantity, salesOrderId }).SingleOrDefault();

                    foreach (var price in item.ItemFees)
                    {
                        try
                        {
                            int rowCount = connection.Query<int>(salesCommissionString.ToString(), new { sale.OrderID, item.ItemCode, price.Type, price.Amount, salesItemId }).SingleOrDefault();
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                        }
                    }


                    foreach (var price in item.ItemPrice)
                    {
                        try
                        {
                            int rowCount = connection.Query<int>(salesPriceString.ToString(), new { sale.OrderID, item.ItemCode, price.Type, price.Amount, salesItemId }).SingleOrDefault();
                        }
                        catch (Exception ex)
                        {
                            message = ex.Message;
                        }
                    }
                }

                return true;
            }
        }

        public bool SynchronizeSKU()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("UPDATE OI ");
            sb.Append("SET OI.SKUItemID = UP.SKUItemID ");
            sb.Append("FROM OrderItems OI ");
            sb.Append("INNER JOIN(Select DISTINCT ItemID,SKUItemID from OrderItems WHERE SKUItemID IS NOT NULL) UP ");
            sb.Append("ON OI.ItemID = UP.ItemID ");

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                connection.Execute(sb.ToString());

            }

            return true;
        }

        public List<SellerItemLink> GetStockErrors()
        {
            var links = new List<SellerItemLink>();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = @"Select DISTINCT ListingItemNo,SKU,o.SellerID,SellerItemName as ItemName,o.SellerIndex
                                  FROM ItemStock s INNER JOIN Orders o  on s.OrderReferenceNo = o.OrderReferenceNo  WHERE ItemMasterID = -1";

                links = connection.Query<SellerItemLink>(query).ToList();

            }

            return links;
        }

        public bool RefreshOrderItems(string orderReferenceNo, List<OrderItem> items)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var predicate = Predicates.Field<Order>(f => f.OrderReferenceNo, Operator.Eq, orderReferenceNo);

                var existingOrder = connection.GetList<Order>(predicate).FirstOrDefault();

                if (existingOrder != null)
                {
                    var oipredicate = Predicates.Field<OrderItem>(f => f.OrderID, Operator.Eq, existingOrder.OrderID);

                    connection.Delete<OrderItem>(oipredicate);

                    foreach (var line in items)
                    {
                        line.OrderID = existingOrder.OrderID;

                    }

                    connection.Insert<OrderItem>(items);
                }
            }

            return true;
        }

        public bool InsertOrders(List<Order> orders)
        {
            bool updateCompleted = false;

            ItemRepository _itemRepository = new ItemRepository();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                foreach (var item in orders)
                {
                    try
                    {
                        var predicate = Predicates.Field<Order>(f => f.OrderReferenceNo, Operator.Eq, item.OrderReferenceNo);

                        var existingOrder = connection.GetList<Order>(predicate);

                        if (existingOrder.Count() == 0)
                        {
                            try
                            {
                                var total = item.OrderItems.Sum(s => s.TransactionPrice);

                                if (total > 0)
                                {
                                    int orderID = connection.Insert<Order>(item);

                                    foreach (var line in item.OrderItems)
                                    {
                                        line.OrderID = orderID;
                                        //UPDATE STOCK

                                        string itemIdQuery = @"Select ItemMasterID from SellerItemLink WHERE ListingItemNo=@itemno AND SKU=@sku AND SellerIndex=@index";

                                        int id = connection.Query<int>(itemIdQuery, new { itemno = line.ItemID, sku = line.SKUItemID.Replace("'", "''"), index = item.SellerIndex }).FirstOrDefault();

                                        if (id > 0)
                                        {
                                            _itemRepository.UpdateStock(new ItemStock()
                                            {
                                                ItemMasterID = id,
                                                Notes = string.Format("Order from {0} Order Number {1}", item.SellerID, item.Channel == "EBay" ? item.SalesOrderNumber : item.OrderReferenceNo),
                                                StockOut = line.Quantity,
                                                InSource = -1,
                                                IsActive = true,
                                                OutSource = item.Channel == "EBay" ? 5 : 4,
                                                ListingItemNo = line.ItemID,
                                                SKU = line.SKUItemID,
                                                OrderReferenceNo = item.OrderReferenceNo,
                                                SellerIndex = item.SellerIndex,
                                                SellerItemName = line.ItemTitle
                                            });
                                        }
                                        else
                                        {
                                            _itemRepository.UpdateStock(new ItemStock()
                                            {
                                                ItemMasterID = -1,
                                                Notes = string.Format("Order from {0} Order Number {1}", item.SellerID, item.Channel == "EBay" ? item.SalesOrderNumber : item.OrderReferenceNo),
                                                StockOut = line.Quantity,
                                                IsActive = true,
                                                InSource = -1,
                                                OutSource = item.Channel == "EBay" ? 5 : 4,
                                                ListingItemNo = line.ItemID,
                                                SKU = line.SKUItemID,
                                                OrderReferenceNo = item.OrderReferenceNo,
                                                SellerIndex = item.SellerIndex,
                                                SellerItemName = line.ItemTitle
                                            });
                                        }
                                    }

                                    connection.Insert<OrderItem>(item.OrderItems);
                                }
                            }
                            catch (Exception)
                            {


                            }

                        }
                        else
                        {
                            item.OrderID = existingOrder.First().OrderID;
                            item.AddiotionalNotes = existingOrder.First().AddiotionalNotes;
                            item.ProceedDate = existingOrder.First().ProceedDate;
                            item.IsActive = existingOrder.First().IsActive;
                            item.PostalCarrierID = existingOrder.First().PostalCarrierID;
                            item.CarrierRef = existingOrder.First().CarrierRef;

                            connection.Update<Order>(item);
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }

                updateCompleted = true;
            }

            return updateCompleted;

        }



        public bool CreateListingItems(List<BulkInsert> ItemsInserted)
        {
            foreach (var item in ItemsInserted)
            {
                ListingRequest request = new ListingRequest()
                {
                    AmazonCategoryID = item.AmazonCategoryID,
                    AmazonListingReference1 = "Auto",
                    AmazonListingReference2 = "Auto",
                    AmazonPrice = item.AmazonPrice,
                    AmazonRecommendedTitle1 = item.ItemMiniTitle,
                    AmazonRecommendedTitle2 = item.ItemMiniTitle,
                    EbayCategoryID = item.EBayCategoryID,
                    EBayListingReference1 = "Auto",
                    EBayListingReference2 = "Auto",
                    EBayPrice = item.EBayPrice,
                    EBayRecommendedTitle1 = item.ItemMiniTitle,
                    EBayRecommendedTitle2 = item.ItemMiniTitle,
                    ItemMasterID = item.ItemMasterID,
                    ListingDescription = "Auto Created for " + item.ItemMiniTitle,
                    SpecialInstructions = "Auto Created for " + item.ItemMiniTitle
                };

                int listingRequestID;

                if (this.AddListing(request, out listingRequestID))
                {
                    var addedListing = this.GetListingRequestByID(listingRequestID);

                    ListingSubmission submission = new ListingSubmission()
                    {
                        ItemMiniTitle = item.ItemMiniTitle,
                        ListingChannelID = item.ListingChannelID,
                        ListingItemNo = item.ListingItemNo,
                        ListingItemPrice = item.ListingChannelID == 1 ? item.EBayPrice : item.AmazonPrice,
                        ListingLink = item.ListingLink,
                        ListingRequestID = listingRequestID,
                        ListingStatus = item.ListingStatus,
                    };

                    this.SubmitListing(submission);
                }
            }

            return true;
        }

        public bool DeleteOrder(int orderID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("Update Orders Set IsActive=0 where OrderID={0}", orderID);
                int rowsAffected = connection.Execute(query, commandType: CommandType.Text);
                return rowsAffected > 0;
            }
        }
    }
}
