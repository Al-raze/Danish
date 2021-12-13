using Infrastrucutre.Core.Models;
using Infrastrucutre.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Provider
{
    public interface IItemProvider
    {
        List<FBALocations> InventoryLocation(); //Inventory Location Like UK EU And USA 18-02-2021
        List<ItemMaster> GetItems(string itemName="");

        bool AddItems(ItemMaster item);
        bool AddFBARequest(FbaRequest item);

        ItemMaster GetItemsByID(int itemID);
        ItemMaster GetsItemsByID(int itemID);//Get Items And Qty For FBA Request

        bool UpdateItems(ItemMaster item, int userId);
        bool UpdateFBARequest(FbaRequest item,int userId);

        List<Supplier> GetSuppliers();

        List<ItemCategory> GetCategories();

        List<ItemManufacturer> GetManufacturers();

        List<ItemColor> GetColors();

        List<ItemLocation> GetLocations();

        bool AddItemCategory(ItemCategory category);

        bool UpdateItemCategory(ItemCategory category);

        ItemCategory GetCategoryByID(int categoryID);

        ItemColor GetItemColorByID(int colorID);

        bool AddItemColor(ItemColor color);

        bool UpdateItemColor(ItemColor color);


        ItemLocation GetItemLocationByID(int locationID);

        bool AddItemLocation(ItemLocation location);

        bool UpdateItemLocation(ItemLocation location);

        ItemManufacturer GetItemManufacturerByID(int manufacturerID);

        bool AddItemManufacturer(ItemManufacturer manufacturer);

        bool UpdateItemManufacturer(ItemManufacturer manufacturer);

        ItemMaster GetItemByID(int id);


        bool AddAmazonItemCategory(AmazonCategory category);

        bool AddEbayItemCategory(EbayCategory category);

        List<EbayCategory> GetEbayCategories();

        EbayCategory GetEbayCategoryByID(int categoryID);

        bool UpdateEbayItemCategory(EbayCategory category);

        List<AmazonCategory> GetAmazonCategories();

        AmazonCategory GetAmazonCategoryByID(int categoryID);

        bool UpdateAmazonItemCategory(AmazonCategory category);

        List<ItemMaster> GetItemNames();

        ItemMaster GetItemInfoByID(int itemID);

        List<ItemMaster> GetItemsBySupplier(int supplierID);

        bool UpdateItemStock(OrderRequest items);

        List<BulkInsert> CreateListings(int[] items);

        bool DeleteItem(int itemMasterID);
        bool DeleteFBARequest(int FBARequestID); // Delete FBA Request 
         

        List<ReportSummary> GetSalesReport(string fromDate, string toDate);
       //---------------------------------------SKU Report 30-11-2020 DANISH BAGWAN ---------------------------------
        List<SKUReport> GetSKUReport(string fromDate, string toDate,string Asin , string SKU, string ItemMasterID, string SellerID);
        List<Asin_Report> Get_AsinReport(string fromDate, string toDate, string Asin, string ItemMasterID);//Get Asin report
        List<AsinSub_Report> Get_AsinSubReport(string fromDate, string toDate, string Asin);//Get Item Data Report 

        List<ItemSummy> Get_ReportItemSummary(string fromDate, string toDate, string Asin);
        List<Sku_Report> Get_SKUReport(string fromDate, string toDate, string Asin, string SKU, string SellerID); // Get Summary SKU DATA
        List<ItemMaster> GetAsinByID(int ItemMasterID); //SKU By ItemMasterID
        List<ItemMaster> GetUserItemByID(int UserID); //User Item Grouping 25-02-2021
        List<ItemMaster> GetFNSKUListByID(string SKU=""); //FNSKU By Item 
        List<ItemMaster> GetSKUByID(string ListingItemNo = ""); //SKU By ItemMasterID
        List<ItemMaster> GetItemsListByID(int SellerID );
        List<GroupData> Get_GroupByReport(string fromDate, string toDate, string UserId); //Group By Report 02-12-2021
        List<GroupData> Get_GroupSummary(string fromDate, string toDate, string UserId); //Group By Report 02-12-2021
        List<GroupData> Get_SumGroupReport(string fromDate, string toDate, string UserId); //Group By Report 03-12-2021
        //-------------------------------------------------------------------------------------------------------------
        List<ItemMaster> GetItems(out int totalCount, string itemName = "", string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0);

        bool QuickUpdate(ItemMaster item);

        StockSourceList GetStockSource();

        StockView UpdateStock(ItemStock item);

        List<ItemStockHistory> GetStockHistory(int id, out int rowCount, int jtStartIndex = 0, int jtPageSize = 0);

        List<ItemMaster> GetItemsWithStock(out int rowCount, string id, string filter, string filterText, int jtStartIndex, int jtPageSize);

        List<StockOut> GetStockOut();

        StockOut AddStockOut(StockOut option);

        bool UpdateStockOut(StockOut option);

        List<AutoCompleteItem> GetItemNames(string itemName = "", bool onlyItemName = false);

        List<AutoCompleteItem> GetSupplierNames(string supplierName = "");

        List<ItemHistory> GetItemHistory(int id);
        List<PostalCarrier> GetPostage(); // Postage Load On Stock Sync Fix Error 15-02-2021
    }
}
