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
using Infrastrucutre.Core.Models.ViewModels;
using System.Reflection;

namespace Infrastrucutre.Core.DataAccess
{



    public class ItemRepository : IItemRepository
    {
        //Removing Entity Framework references
        //RepositoryContext db = new RepositoryContext();

        public StockOut AddStockOut(StockOut option)
        {
            var record = new StockOut();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                int id = connection.Insert<StockOut>(option);

                record = connection.Get<StockOut>(id);
            }

            return record;

        }

        public bool UpdateStockOut(StockOut option)
        {

            bool updated = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updated = connection.Update<StockOut>(option);
            }

            return updated;
        }

        public List<AutoCompleteItem> GetSupplierNames(string supplierName = "")
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("Select SupplierName as label,SupplierID as id  from Suppliers WHERE SupplierName LIKE '{0}%'", supplierName);
                IEnumerable<AutoCompleteItem> items = connection.Query<AutoCompleteItem>(query, parameters, commandType: CommandType.Text);
                return items.ToList();
            }
        }

        public List<AutoCompleteItem> GetItemNames(string itemName = "", bool onlyItemName = false)
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("Select TOP 10 (ItemName + '-' + ItemCode) as label,ItemMasterID as id from ItemMasters Where IsActive=1  AND itemName LIKE '{0}%'", itemName);
                if (onlyItemName)
                {
                    query = string.Format("Select TOP 10 ItemName as label,ItemMasterID as id from ItemMasters Where IsActive=1  AND itemName LIKE '{0}%'", itemName);
                }
                IEnumerable<AutoCompleteItem> items = connection.Query<AutoCompleteItem>(query, parameters, commandType: CommandType.Text);
                return items.ToList();
            }
        }

        public StockSourceList GetStockSource() //8ugkcfcy
        {
            string query = @"Select SupplierID as [Id],SupplierName as [Value],'IN' As [Type] from Suppliers
                            UNION ALL
                            Select Id,Source,'OUT' from StockOut";

            var source = new StockSourceList();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                List<StockSource> items = connection.Query<StockSource>(query).ToList();

                source.Purchase = items.Where(i => i.Type == "IN").ToList();
                source.Sale = items.Where(i => i.Type == "OUT").ToList();

            }

            return source;
        }

        public StockView UpdateStock(ItemStock stock)
        {
            int updated;

            var view = new StockView();

            view.IsUpdated = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                stock.IsActive = true;

                updated = connection.Insert<ItemStock>(stock);

                var predicate = Predicates.Field<StockView>(f => f.ItemMasterID, Operator.Eq, stock.ItemMasterID);

                view = connection.GetList<StockView>(predicate).FirstOrDefault();

                view.IsUpdated = true;

            }

            return view;
        }

        public List<ItemStockHistory> GetStockHistory(int id, out int rowCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            string query = string.Format(@"Select stock.*,supplier.SupplierName,source.Source ,master.ItemName,master.ItemCost  from ItemStock stock 
                             inner join ItemMasters master on stock.ItemMasterID = master.ItemMasterID
                            left join Suppliers supplier on supplier.SupplierID = stock.InSource
                            left join StockOut source on  source.Id = stock.OutSource 
                            WHERE stock.ItemMasterID = @id 
                            ORDER BY StockId DESC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", jtStartIndex, jtPageSize);

            string countQuery = @"Select count(*) from ItemStock stock 
                                  inner join ItemMasters master on stock.ItemMasterID = master.ItemMasterID
                                  WHERE stock.ItemMasterID = @id;";
            rowCount = 0;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var reader = connection.QueryMultiple(countQuery + query, new { id = id });

                rowCount = reader.Read<int>().Single();

                var result = reader.Read<ItemStockHistory>();

                return result.ToList();
            }
        }

        public List<ItemMaster> GetItems(string itemName = "")
        {
            var parameters = new DynamicParameters();

            parameters.Add("@ItemName", itemName, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Item_GetAllItems";
                IEnumerable<ItemMaster> items = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return items.ToList();
            }
        }


        public List<StockOut> GetStockOut()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var list = connection.GetList<StockOut>();

                return list.ToList();
            }

        }


        public List<ItemMaster> GetItemsWithStock(out int totalCount, string itemName = "", string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0)
        {
            var parameters = new DynamicParameters();

            string whereClause = string.Empty;

            bool isStockQuery = false;

            itemName = string.IsNullOrWhiteSpace(itemName) ? "" : itemName.Trim();
            filter = string.IsNullOrWhiteSpace(filter) ? "" : filter.Trim();
            filterText = string.IsNullOrWhiteSpace(filterText) ? "" : filterText.Trim();

            if (filter != "" || itemName != "")
            {
                whereClause = " WHERE ";
                if (filter == "ItemName" || itemName != "")
                {
                    itemName = itemName == "" ? filterText : itemName;
                    whereClause += "IM.ItemName LIKE '%' + @ItemName + '%'";
                    parameters.Add("@ItemName", itemName, DbType.String, ParameterDirection.Input);
                }

                if (filter == "ItemCode")
                {
                    whereClause += "IM.ItemCode LIKE '%' + @ItemCode + '%'";
                    parameters.Add("@ItemCode", filterText, DbType.String, ParameterDirection.Input);
                }

                if (filter == "Supplier")
                {
                    whereClause += "S.SupplierName LIKE '%' + @SupplierName + '%'";
                    parameters.Add("@SupplierName", filterText, DbType.String, ParameterDirection.Input);
                }

                int stock;

                if (filter == "AvaliableStockGreater")
                {
                    if (int.TryParse(filterText, out stock))
                    {
                        whereClause += "ISNULL(stock.CurrentStock,0) >=" + filterText;
                    }
                    else
                    {
                        whereClause += "ISNULL(stock.CurrentStock,0) >=0";
                    }

                    isStockQuery = true;
                }

                if (filter == "AvaliableStockLesser")
                {
                    if (int.TryParse(filterText, out stock))
                    {
                        whereClause += "ISNULL(stock.CurrentStock,0) <=" + filterText;
                    }
                    else
                    {
                        whereClause += "ISNULL(stock.CurrentStock,0) <=0";
                    }
                    isStockQuery = true;
                }

                if (filter == "LowStock")
                {
                    whereClause += "ISNULL(stock.CurrentStock,0) <= IM.ReOrderLevel";
                    isStockQuery = true;
                }
            }


            string countQuery = string.Format(@"SELECT COUNT(*) FROM 
		                        ItemMasters IM 
                                INNER JOIN ItemCategories IC on IC.ItemCategoryID = IM.ItemCategoryID AND IM.IsActive=1
		                        INNER JOIN ItemColors ICO on ICO.ItemColorID = IM.ItemColorID
		                        INNER JOIN ItemManufacturers IMF on IMF.ItemManufacturerID = IM.ItemManufacturerID
                                INNER JOIN Suppliers S on S.SupplierID = IM.SupplierID                                
                                {0}", whereClause);

            if (isStockQuery)
            {
                countQuery = string.Format(@"SELECT COUNT(*) FROM 
		                        ItemMasters IM 
                                INNER JOIN ItemCategories IC on IC.ItemCategoryID = IM.ItemCategoryID AND IM.IsActive=1
		                        INNER JOIN ItemColors ICO on ICO.ItemColorID = IM.ItemColorID
		                        INNER JOIN ItemManufacturers IMF on IMF.ItemManufacturerID = IM.ItemManufacturerID
                                INNER JOIN Suppliers S on S.SupplierID = IM.SupplierID
                                LEFT JOIN StockView stock on stock.ItemMasterID = IM.ItemMasterID
                                {0};", whereClause);
            }

            string query = string.Format(@"SELECT IM.ItemMasterID,ItemName, ItemCode, [Description], 
                            Brand, Dimension, ItemWeight,IM.ItemCost , VAT, 
                            TotalCost, BarCode, IM.SupplierID, IM.ItemCategoryID, IM.ItemManufacturerID, IM.ItemColorID,
		                    IC.CategoryName,ICO.Color,IMF.ManufacturerName,S.SupplierName,IM.StockUnits,IM.ReOrderLevel,
		                    ActiveListingCount.ListingCount as ActiveListings,stock.StockIn,stock.StockOut,stock.CurrentStock
		                    FROM 
		                    ItemMasters IM 
							INNER JOIN ItemStock I on I.ItemMasterID = IM.ItemMasterID
		                    INNER JOIN ItemCategories IC on IC.ItemCategoryID = IM.ItemCategoryID AND IM.IsActive=1
		                    INNER JOIN ItemColors ICO on ICO.ItemColorID = IM.ItemColorID
		                    INNER JOIN ItemManufacturers IMF on IMF.ItemManufacturerID = IM.ItemManufacturerID
		                    INNER JOIN Suppliers S on S.SupplierID = IM.SupplierID
		                    LEFT JOIN vw_ListingLinkCount ActiveListingCount on ActiveListingCount.ItemMasterID = IM.ItemMasterID                          
                            LEFT JOIN StockView stock on stock.ItemMasterID = IM.ItemMasterID
                            {2}
                            ORDER BY IM.ItemMasterID DESC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", jtStartIndex, jtPageSize, whereClause);




            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                var reader = connection.QueryMultiple(countQuery + query, parameters);

                totalCount = reader.Read<int>().Single();

                var result = reader.Read<ItemMaster>();

                return result.ToList();
            }

        }
        


        public List<ItemHistory> GetItemHistory(int id)
        {
            var list = new List<ItemHistory>();

            string query = @"Select  IM.*,IC.CategoryName,S.SupplierName,IMF.ManufacturerName,ICO.Color,Users.UserName from ItemMaster_History IM
                            INNER JOIN ItemCategories IC on IC.ItemCategoryID = IM.ItemCategoryID 
                            INNER JOIN ItemColors ICO on ICO.ItemColorID = IM.ItemColorID
                            INNER JOIN ItemManufacturers IMF on IMF.ItemManufacturerID = IM.ItemManufacturerID
                            INNER JOIN Suppliers S on S.SupplierID = IM.SupplierID
                            LEFT JOIN AppUsers Users on Users.UserID = IM.ModifiedByUser
                            WHERE IM.ItemMasterID=@id
                            ORDER BY IM.ItemMasterHistoryID DESC";

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                list = connection.Query<ItemHistory>(query, new { id }).ToList();
            }

            return list;

        }


        public List<ItemMaster> GetItems(out int totalCount, string itemName = "", string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0)
        {
            var parameters = new DynamicParameters();

            string whereClause = string.Empty;


            itemName = string.IsNullOrWhiteSpace(itemName) ? "" : itemName.Trim();
            filter = string.IsNullOrWhiteSpace(filter) ? "" : filter.Trim();
            filterText = string.IsNullOrWhiteSpace(filterText) ? "" : filterText.Trim();

            if (filter != "" || itemName != "")
            {
                whereClause = " WHERE ";
                if (filter == "ItemName" || itemName != "")
                {
                    itemName = itemName == "" ? filterText : itemName;
                    whereClause += "IM.ItemName LIKE '%' + @ItemName + '%'";
                    parameters.Add("@ItemName", itemName, DbType.String, ParameterDirection.Input);
                }

                if (filter == "ItemCode")
                {
                    whereClause += "IM.ItemCode LIKE '%' + @ItemCode + '%'";
                    parameters.Add("@ItemCode", filterText, DbType.String, ParameterDirection.Input);
                }

                if (filter == "Notes")
                {
                    whereClause += "IM.Notes LIKE '%' + @Notes + '%'";
                    parameters.Add("@Notes", filterText, DbType.String, ParameterDirection.Input);
                }

                if (filter == "Supplier")
                {
                    whereClause += "S.SupplierName LIKE '%' + @SupplierName + '%'";
                    parameters.Add("@SupplierName", filterText, DbType.String, ParameterDirection.Input);
                }

                if (filter == "ListingCount")
                {
                    int count = 0;

                    if (int.TryParse(filterText.Trim(), out count))
                    {
                        whereClause += "ActiveListingCount.ListingCount>=@listingcount";
                        parameters.Add("@listingcount", count, DbType.Int32, ParameterDirection.Input);
                    }
                }

            }


            string countQuery = string.Format(@"SELECT COUNT(*) FROM 
		                        ItemMasters IM 
		                        INNER JOIN ItemCategories IC on IC.ItemCategoryID = IM.ItemCategoryID AND IM.IsActive=1
		                        INNER JOIN ItemColors ICO on ICO.ItemColorID = IM.ItemColorID
		                        INNER JOIN ItemManufacturers IMF on IMF.ItemManufacturerID = IM.ItemManufacturerID
                                INNER JOIN Suppliers S on S.SupplierID = IM.SupplierID
                                LEFT JOIN vw_ListingLinkCount ActiveListingCount on ActiveListingCount.ItemMasterID = IM.ItemMasterID
                                {0}", whereClause);

            string query = string.Format(@"SELECT IM.ItemMasterID,ItemName, ItemCode, [Description], 
                            Brand, Dimension, ItemWeight,IM.ItemCost,VAT, 
		                    TotalCost, BarCode, IM.SupplierID, IM.ItemCategoryID, IM.ItemManufacturerID, IM.ItemColorID,
		                    IC.CategoryName,ICO.Color,IMF.ManufacturerName,S.SupplierName,IM.StockUnits,IM.ReOrderLevel,
		                    ActiveListingCount.ListingCount AS ActiveListings
		                    FROM 
		                    ItemMasters IM 
		                    INNER JOIN ItemCategories IC on IC.ItemCategoryID = IM.ItemCategoryID AND IM.IsActive=1
		                    INNER JOIN ItemColors ICO on ICO.ItemColorID = IM.ItemColorID
		                    INNER JOIN ItemManufacturers IMF on IMF.ItemManufacturerID = IM.ItemManufacturerID
		                    INNER JOIN Suppliers S on S.SupplierID = IM.SupplierID
		                    LEFT JOIN vw_ListingLinkCount ActiveListingCount on ActiveListingCount.ItemMasterID = IM.ItemMasterID
                            {2}
                            ORDER BY IM.ItemMasterID DESC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", jtStartIndex, jtPageSize, whereClause);




            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                var reader = connection.QueryMultiple(countQuery + "\n" + query, parameters);

                totalCount = reader.Read<int>().Single();

                var result = reader.Read<ItemMaster>();

                return result.ToList();
            }

        }

        public bool DeleteItem(int itemMasterID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("Update ItemMasters Set IsActive=0 where ItemMasterID={0}", itemMasterID);
                int rowsAffected = connection.Execute(query, commandType: CommandType.Text);
                return rowsAffected > 0;
            }

        }

        public List<ItemMaster> GetItemNames()
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string query = "Select ItemName,ItemMasterID from ItemMasters Where IsActive=1";
                IEnumerable<ItemMaster> items = connection.Query<ItemMaster>(query, parameters, commandType: CommandType.Text);
                return items.ToList();
            }
        }

        public List<PostalCarrier> GetPostage() // Postage Carrier Name Load By D 15-02-2021
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string query = "Select PostalCarrierName,PostalCarrierID from PostalCarriers";
                IEnumerable<PostalCarrier> items = connection.Query<PostalCarrier>(query, parameters, commandType: CommandType.Text);
                return items.ToList();
            }
        }

        public bool AddItems(ItemMaster item) //-----
        {

            var parameters = new DynamicParameters();

            parameters.Add("@ItemName", item.ItemName, DbType.String, ParameterDirection.Input);
            parameters.Add("@ItemCode", item.ItemCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@ItemDescription", item.Description, DbType.String, ParameterDirection.Input);
            parameters.Add("@Brand", item.Brand, DbType.String, ParameterDirection.Input);
            parameters.Add("@Dimension", item.Dimension, DbType.String, ParameterDirection.Input);
            parameters.Add("@ItemWeight", item.ItemWeight, DbType.String, ParameterDirection.Input);
            parameters.Add("@VAT", item.VAT, DbType.String, ParameterDirection.Input);
            parameters.Add("@ItemCost", item.ItemCost, DbType.String, ParameterDirection.Input);
            parameters.Add("@BarCode", item.BarCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@SupplierID", item.SupplierID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ItemCategoryID", item.ItemCategoryID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ItemManufacturerID", item.ItemManufacturerID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ItemColorID", item.ItemColorID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@LocationID", item.LocationID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@StockUnits", 0, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ReOrderLevel", item.ReOrderLevel, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@MasterCartonQTY", item.MasterCartonQty, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@LeadTime", item.LeadTime, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@FNSKU", item.FNSKU, DbType.String, ParameterDirection.Input);
            parameters.Add("@Notes", item.Notes, DbType.String, ParameterDirection.Input);
            parameters.Add("@DateOfSubmission", item.DateOfSubmission, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("@Length", item.Length, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Width", item.Width, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@Height", item.Height, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@SellingPrice", item.SellingPrice, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Item_InsertItem";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                //int rowsaffected = connection.Insert<ItemMaster>(item, transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool AddFBARequest(FbaRequest item) //-----
        {

            var parameters = new DynamicParameters();

            parameters.Add("@UserID", item.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@ItemMasterID", item.ItemMasterID, DbType.String, ParameterDirection.Input);
            parameters.Add("@SellerIndex", item.SellerIndex, DbType.String, ParameterDirection.Input);
            parameters.Add("@ListinItemNo", item.ListingItemNo, DbType.String, ParameterDirection.Input);
            parameters.Add("@SKU", item.SKU, DbType.String, ParameterDirection.Input);
            parameters.Add("@FNSKU", item.FNSKU, DbType.String, ParameterDirection.Input);
            parameters.Add("@FBARootID", item.FBARootID, DbType.String, ParameterDirection.Input);
            parameters.Add("@FBARecedQty", item.FBARecedQty, DbType.String, ParameterDirection.Input);
            parameters.Add("@RequestQty", item.RequestQty, DbType.String, ParameterDirection.Input);
            parameters.Add("@Comments", item.Comments, DbType.String, ParameterDirection.Input);
            parameters.Add("@LableLink", item.LableLink, DbType.String, ParameterDirection.Input);
            parameters.Add("@LableStatus", item.LableStatus, DbType.String, ParameterDirection.Input);
            parameters.Add("@PriorityStatus", item.PriorityStatus, DbType.String, ParameterDirection.Input);
            parameters.Add("@UKSold7Days", item.UKSold7Days, DbType.String, ParameterDirection.Input);
            parameters.Add("@EUSold7Days", item.EUSold7Days, DbType.String, ParameterDirection.Input);
            parameters.Add("@USASold7Days", item.USASold7Days, DbType.String, ParameterDirection.Input);
            parameters.Add("@UKSold30Days", item.UKSold30Days, DbType.String, ParameterDirection.Input);
            parameters.Add("@EUSold30Days", item.EUSold30Days, DbType.String, ParameterDirection.Input);
            parameters.Add("@USASold30Days", item.USASold30Days, DbType.String, ParameterDirection.Input);
            parameters.Add("@UKFbaStock", item.UKFbaStock, DbType.String, ParameterDirection.Input);



            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.FBA_InsertFBA";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                //int rowsaffected = connection.Insert<ItemMaster>(item, transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool UpdateFbaRequest(FbaRequest item, int userId)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<FbaRequest>(item);
            }

            return updateCompleted;
        }
        public bool DeleteFBARequest(int FBARequestID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("Update FBARequest Set Del='Y' where FBARequestID={0}", FBARequestID);
                int rowsAffected = connection.Execute(query, commandType: CommandType.Text);
                return rowsAffected > 0;
            }
            
        }

        public bool UpdateItemStock(OrderRequest items)
        {
            int rowsaffected = 0;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                IDbTransaction transaction = connection.BeginTransaction();

                foreach (var item in items.ItemsRequested)
                {
                    string updateQuery = string.Format("Update ItemMasters Set StockUnits={0} Where ItemMasterID={1}", item.RequestedItemQuantity, item.ItemMasterID);
                    rowsaffected += connection.Execute(updateQuery, commandType: CommandType.Text, transaction: transaction);

                }

                transaction.Commit();

                return rowsaffected == items.ItemsRequested.Count;
            }
        }

        public ItemMaster GetItemsByID(int itemID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                ItemMaster item = connection.Get<ItemMaster>(itemID);

                return item;
            }

        }
        //--------------------------------------Report stored procedure 30-11-2020---------------------------------------//

        public List<SKUReport> GetSKUReport(string fromDate, string toDate,string Asin, string SKU, string ItemMasterID, string SellerID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<SKUReport>("SKU_Report", new { fromDate = fromDate, toDate = toDate, Asin = Asin, SKU=SKU, ItemMasterID = ItemMasterID, SellerID= SellerID }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public List<Sku_Report> Get_SKUReport(string fromDate, string toDate, string Asin, string SKU, string SellerID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<Sku_Report>("Get_SkuReport", new { fromDate = fromDate, toDate = toDate, Asin = Asin, SKU = SKU, SellerID= SellerID }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public List<Asin_Report> Get_AsinReport(string fromDate, string toDate, string Asin,string ItemMasterID) //Asin Report 
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<Asin_Report>("Get_AsinReport", new { fromDate = fromDate, toDate = toDate, Asin = Asin ,ItemMasterID = ItemMasterID}, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public List<AsinSub_Report> Get_AsinSubReport(string fromDate, string toDate, string Asin) //Item  Data  
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
              
                 var result = connection.Query<AsinSub_Report>("Get_AsinSubReport", new { fromDate = fromDate, toDate = toDate, Asin = Asin }, commandType: CommandType.StoredProcedure);
                
                return result.ToList();
            }
        }

        public List<ItemSummy> Get_ReportItemSummary(string fromDate, string toDate, string Asin) //Item Summary Data 
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<ItemSummy>("Get_ItemSummaryReport", new { fromDate = fromDate, toDate = toDate, Asin = Asin }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        //-----------------------------------GROUP BY REPORT 02-12-2021 ----------------------------------//

        public List<GroupData> Get_GroupByReport(string fromDate, string toDate, string UserId) //Item Summary Data 
        {

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<GroupData>("Get_GroupByReport", new { fromDate = fromDate, toDate = toDate, UserId = UserId }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public List<GroupData> Get_GroupSummary(string fromDate, string toDate, string UserId) //Item Summary Data 
        {

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<GroupData>("Get_GroupSummary", new { fromDate = fromDate, toDate = toDate, UserId = UserId }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public List<GroupData> Get_SumGroupReport(string fromDate, string toDate, string UserId) //Item Summary Data 
        {

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<GroupData>("Get_GroupSum", new { fromDate = fromDate, toDate = toDate, UserId = UserId }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
        //------------------------------------------------------  ------------------------------------------------------//

        //______________________________________________________________GET SKU BY ITEM_______________________________________________________________

        public List<ItemMaster> GetAsinByID(int ItemMasterID = 0)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@ItemMasterID", ItemMasterID, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.GetAsinByID";
                IEnumerable<ItemMaster> AsinList = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return AsinList.ToList();
            }

        }

        public List<ItemMaster> GetUserItemByID(int UserID = 0)
        {
            

            var parameters = new DynamicParameters();
            parameters.Add("@UserID", UserID, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.GetUserItemByID";
                IEnumerable <ItemMaster> AsinList = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return AsinList.ToList();
                }
            
        }

        public List<ItemMaster> GetFNSKUListById(string SKU ="") 
        {

            var parameters = new DynamicParameters();
            parameters.Add("@ItemMasterID", SKU, DbType.String, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.GetFNSKUByID";
                IEnumerable<ItemMaster> AsinList = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return AsinList.ToList();
            }
        }

        public List<ItemMaster> GetSKUByID(string ListingItemNo )
        {


            var parameters = new DynamicParameters();
            parameters.Add("@ListingItemNo", ListingItemNo, DbType.String, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.GetSKUByID";
                IEnumerable<ItemMaster> SKUList = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return SKUList.ToList();
            }

        }

        public List<ItemMaster> GetItemsListByID(int SellerID)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@SellerID", SellerID, DbType.String, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.GetItemsListByID";
                IEnumerable<ItemMaster> ItemsList = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return ItemsList.ToList();
            }

        }
        //________________________________________________________18-02-2021________________________________________________________________//

        public List<FBALocations> InventoryLocation()
        {
                var parameters = new DynamicParameters();

                using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
                {
                    string query = string.Format("Select FBARootID,FBARoot  from FBARoot Where Del = 'N' ");
                    IEnumerable<FBALocations> items = connection.Query<FBALocations>(query, parameters, commandType: CommandType.Text);
                    return items.ToList();
                }

        }

        //____________________________________________________________________________________________________________________________________________
        public List<ReportSummary> GetSalesReport(string fromDate, string toDate)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var result = connection.Query<ReportSummary>("Sales_Report", new { fromDate = fromDate, toDate = toDate }, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }

        public bool InsertHistory(ItemMaster item, int userId = 0)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                ItemHistory history = new ItemHistory();

                CopyPropertiesTo(item, history);

                history.ModifiedByUser = userId;
                history.ModifiedDate = DateTime.Now;

                connection.Insert<ItemHistory>(history);

                return true;
            }
        }

        public static void CopyPropertiesTo(object fromObject, object toObject)
        {
            PropertyInfo[] toObjectProperties = toObject.GetType().GetProperties();
            foreach (PropertyInfo propTo in toObjectProperties)
            {
                PropertyInfo propFrom = fromObject.GetType().GetProperty(propTo.Name);
                if (propFrom != null && propFrom.CanWrite)
                    propTo.SetValue(toObject, propFrom.GetValue(fromObject, null), null);
            }
        }

        public bool UpdateItems(ItemMaster item, int userId)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<ItemMaster>(item);
            }

            InsertHistory(item, userId);

            return updateCompleted;
        }

        public bool QuickUpdate(ItemMaster item)
        {
            int rows = 0;

            var history = new ItemMaster();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                rows = connection.Execute("Update ItemMasters SET StockUnits=@units WHERE ItemMasterID=@id", new { units = 0, id = item.ItemMasterID });

                history = connection.Get<ItemMaster>(item.ItemMasterID);
            }

            if (rows > 0)
                InsertHistory(history);

            return rows > 0;
        }




        public bool DeleteIem(int itemID)
        {

            bool deleteCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var predicate = Predicates.Field<ItemMaster>(i => i.ItemMasterID, Operator.Eq, true);
                deleteCompleted = connection.Delete<ItemMaster>(predicate);
            }

            return deleteCompleted;
        }

        public List<Supplier> GetSuppliers()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<Supplier> items = connection.GetList<Supplier>().ToList();

                return items;
            }
        }

        public List<ItemCategory> GetCategories()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<ItemCategory> items = connection.GetList<ItemCategory>().ToList();

                return items;
            }
        }

        public ItemCategory GetCategoryByID(int categoryID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                ItemCategory item = connection.Get<ItemCategory>(categoryID);

                return item;
            }
        }

        public bool AddItemCategory(ItemCategory category)
        {
            int categoryID;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                categoryID = connection.Insert<ItemCategory>(category);

                return categoryID > 0;
            }
        }

        public bool UpdateItemCategory(ItemCategory category)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<ItemCategory>(category);

                return updateCompleted;
            }
        }


        public List<ItemManufacturer> GetManufacturers()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<ItemManufacturer> items = connection.GetList<ItemManufacturer>().ToList();

                return items;
            }
        }

        public ItemManufacturer GetItemManufacturerByID(int manufacturerID)
        {

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                ItemManufacturer item = connection.Get<ItemManufacturer>(manufacturerID);

                return item;
            }

        }
        public bool AddItemManufacturer(ItemManufacturer manufacturer)
        {

            int manufacturerID;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                manufacturerID = connection.Insert<ItemManufacturer>(manufacturer);

                return manufacturerID > 0;
            }

        }
        public bool UpdateItemManufacturer(ItemManufacturer manufacturer)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<ItemManufacturer>(manufacturer);

                return updateCompleted;
            }

        }




        public List<ItemColor> GetColors()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<ItemColor> items = connection.GetList<ItemColor>().ToList();

                return items;
            }
        }


        public ItemColor GetItemColorByID(int colorID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                ItemColor item = connection.Get<ItemColor>(colorID);

                return item;
            }
        }

        public bool AddItemColor(ItemColor color)
        {
            int colorID;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                colorID = connection.Insert<ItemColor>(color);

                return colorID > 0;
            }
        }

        public bool UpdateItemColor(ItemColor color)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<ItemColor>(color);

                return updateCompleted;
            }
        }



        public List<ItemLocation> GetLocations()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<ItemLocation> items = connection.GetList<ItemLocation>().ToList();

                return items;
            }
        }


        public ItemLocation GetItemLocationByID(int locationID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                ItemLocation item = connection.Get<ItemLocation>(locationID);

                return item;
            }
        }

        public bool AddItemLocation(ItemLocation location)
        {
            int locationID;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                locationID = connection.Insert<ItemLocation>(location);

                return locationID > 0;
            }
        }

        public bool UpdateItemLocation(ItemLocation location)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<ItemLocation>(location);

                return updateCompleted;
            }
        }

        public ItemMaster GetItemByID(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ItemID", id, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Item_GetItemByID";
                IEnumerable<ItemMaster> items = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return items.FirstOrDefault();
            }
        }
        

        public List<ItemMaster> GetItemsBySupplier(int supplierID = 0)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SupplierID", supplierID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Item_GetAllItemsBySupplier";
                IEnumerable<ItemMaster> items = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return items.ToList();
            }
        }

        public List<EbayCategory> GetEbayCategories()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<EbayCategory> items = connection.GetList<EbayCategory>().ToList();

                return items;
            }
        }

        public EbayCategory GetEbayCategoryByID(int categoryID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                EbayCategory item = connection.Get<EbayCategory>(categoryID);

                return item;
            }
        }

        public bool AddEbayItemCategory(EbayCategory category)
        {
            int categoryID;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                categoryID = connection.Insert<EbayCategory>(category);

                return categoryID > 0;
            }
        }

        public bool UpdateEbayItemCategory(EbayCategory category)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<EbayCategory>(category);

                return updateCompleted;
            }
        }



        public List<AmazonCategory> GetAmazonCategories()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<AmazonCategory> items = connection.GetList<AmazonCategory>().ToList();

                return items;
            }
        }

        public AmazonCategory GetAmazonCategoryByID(int categoryID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                AmazonCategory item = connection.Get<AmazonCategory>(categoryID);

                return item;
            }
        }

        public bool AddAmazonItemCategory(AmazonCategory category)
        {
            int categoryID;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                categoryID = connection.Insert<AmazonCategory>(category);

                return categoryID > 0;
            }
        }

        public bool UpdateAmazonItemCategory(AmazonCategory category)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<AmazonCategory>(category);

                return updateCompleted;
            }
        }

        public ItemMaster GetItemInfoByID(int itemID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@ItemMasterID", itemID, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Listing_GetItemInfoByID";
                IEnumerable<ItemMaster> items = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return items.FirstOrDefault();
            }
        }

        public ItemMaster GetsItemsByID(int itemID)
        {


            var parameters = new DynamicParameters();
            parameters.Add("@ItemMasterID", itemID, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.GetItemsInfoByID";
                IEnumerable<ItemMaster> items = connection.Query<ItemMaster>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return items.FirstOrDefault();
            }

            //           var Items = new List<ItemMaster>();
            //           using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            //           {
            //               string query = @"Select I.ItemName,I.ItemMasterID,M.ManufacturerName,(cast(Length as varchar(10)) + 'x' + cast(Width as varchar(10)) + 'x' + cast(Height as varchar(10))) As Dimension1, I.CartonQty,I.MasterCartonQty
            //                                       INTO #TempTable from  ItemMasters I inner join ItemCategories C on I.ItemCategoryID = c.ItemCategoryID inner join
            //                                       ItemManufacturers M on I.ItemManufacturerID = M.ItemManufacturerID WHERE I.ItemMasterID = @itemID
            //                               SELECT *,  

            //                            (Select SUM([OrderItems].[Quantity]) AS Quantity FROM[arsukeuro_mssql].[dbo].[ItemStock]
            //                                JOIN[arsukeuro_mssql].[dbo].[ItemMasters] ON[ItemStock].[ItemMasterID] = [ItemMasters].[ItemMasterID]
            //                                JOIN[arsukeuro_mssql].[dbo].[Orders] ON[Orders].[OrderReferenceNo] = [ItemStock].[OrderReferenceNo]
            //                                JOIN[arsukeuro_mssql].[dbo].[OrderItems] ON[Orders].OrderID = [OrderItems].OrderID
            //                                WHERE OrderDate>= DATEADD(DAY, -7, GETDATE()) AND Orders.Country = 'GB' AND ItemStock.ItemMasterID = @itemID
            //                                GROUP BY[ItemStock].[ItemMasterID] ) AS UKSold7Days,

            //                            (SELECT SUM([OrderItems].[Quantity]) AS Quantity
            //                                FROM[arsukeuro_mssql].[dbo].[ItemStock]
            //                                JOIN[arsukeuro_mssql].[dbo].[ItemMasters] ON[ItemStock].[ItemMasterID] = [ItemMasters].[ItemMasterID]
            //                                JOIN[arsukeuro_mssql].[dbo].[Orders] ON[Orders].[OrderReferenceNo] = [ItemStock].[OrderReferenceNo]
            //                                JOIN[arsukeuro_mssql].[dbo].[OrderItems] ON[Orders].OrderID = [OrderItems].OrderID
            //                                WHERE OrderDate>= DATEADD(DAY, -7, GETDATE()) AND Orders.Country = 'AU' AND ItemStock.ItemMasterID = @itemID
            //                                GROUP BY[ItemStock].[ItemMasterID] ) AS AUSold7Days,

            //                            (SELECT SUM([OrderItems].[Quantity]) AS Quantity

            //       FROM[arsukeuro_mssql].[dbo].[ItemStock]

            //       JOIN[arsukeuro_mssql].[dbo].[ItemMasters] ON[ItemStock].[ItemMasterID] = [ItemMasters].[ItemMasterID]

            //       JOIN[arsukeuro_mssql].[dbo].[Orders] ON[Orders].[OrderReferenceNo] = [ItemStock].[OrderReferenceNo]

            //       JOIN[arsukeuro_mssql].[dbo].[OrderItems] ON[Orders].OrderID = [OrderItems].OrderID

            //       WHERE OrderDate>= DATEADD(DAY, -7, GETDATE()) AND Orders.Country = 'USA' AND ItemStock.ItemMasterID = @itemID

            //       GROUP BY[ItemStock].[ItemMasterID] ) AS USASold7Days,

            //	(Select

            //      SUM([OrderItems].[Quantity]) AS Quantity


            //       FROM[arsukeuro_mssql].[dbo].[ItemStock]

            //       JOIN[arsukeuro_mssql].[dbo].[ItemMasters] ON[ItemStock].[ItemMasterID] = [ItemMasters].[ItemMasterID]

            //       JOIN[arsukeuro_mssql].[dbo].[Orders] ON[Orders].[OrderReferenceNo] = [ItemStock].[OrderReferenceNo]

            //       JOIN[arsukeuro_mssql].[dbo].[OrderItems] ON[Orders].OrderID = [OrderItems].OrderID

            //       WHERE OrderDate>= DATEADD(DAY, -30, GETDATE()) AND Orders.Country = 'GB' AND ItemStock.ItemMasterID = @itemID

            //       GROUP BY[ItemStock].[ItemMasterID] ) AS UKSold30Days,

            //(SELECT SUM([OrderItems].[Quantity]) AS Quantity


            //       FROM[arsukeuro_mssql].[dbo].[ItemStock]

            //       JOIN[arsukeuro_mssql].[dbo].[ItemMasters] ON[ItemStock].[ItemMasterID] = [ItemMasters].[ItemMasterID]

            //       JOIN[arsukeuro_mssql].[dbo].[Orders] ON[Orders].[OrderReferenceNo] = [ItemStock].[OrderReferenceNo]

            //       JOIN[arsukeuro_mssql].[dbo].[OrderItems] ON[Orders].OrderID = [OrderItems].OrderID

            //       WHERE OrderDate>= DATEADD(DAY, -30, GETDATE()) AND Orders.Country = 'AU' AND ItemStock.ItemMasterID = @itemID

            //       GROUP BY[ItemStock].[ItemMasterID] ) AS AUSold30Days,

            //                    (SELECT SUM([OrderItems].[Quantity]) AS Quantity FROM[arsukeuro_mssql].[dbo].[ItemStock]
            //                       JOIN[arsukeuro_mssql].[dbo].[ItemMasters] ON[ItemStock].[ItemMasterID] = [ItemMasters].[ItemMasterID]
            //                       JOIN[arsukeuro_mssql].[dbo].[Orders] ON[Orders].[OrderReferenceNo] = [ItemStock].[OrderReferenceNo]
            //                       JOIN[arsukeuro_mssql].[dbo].[OrderItems] ON[Orders].OrderID = [OrderItems].OrderID
            //                       WHERE OrderDate>= DATEADD(DAY, -30, GETDATE()) AND Orders.Country = 'USA' AND ItemStock.ItemMasterID = @itemID
            //                       GROUP BY[ItemStock].[ItemMasterID] ) AS USASold30Days,

            //   (SELECT StockView.CurrentStock FROM  StockView WHERE StockView.ItemMasterID = @ItemMasterID) AS UKWarehouse



            //     FROM #TempTable WHERE TempTable.ItemMasterID = @item";

            //               Items = connection.Query<ItemMaster>(query).ToList();
            //           }

            //           return Items;


        }



        public List<BulkInsert> CreateListings(int[] items)
        {
            string itemsXml = string.Empty;
            itemsXml += "<root>";

            foreach (var item in items)
            {
                itemsXml += string.Format("<Access ItemID=\"{0}\"/>", item);
            }

            itemsXml += "</root>";
            var parameters = new DynamicParameters();
            parameters.Add("@XmlItemID", itemsXml, DbType.Xml, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Item_CreateListing";
                IEnumerable<BulkInsert> item = connection.Query<BulkInsert>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return item.ToList();
            }
        }
    }
}
