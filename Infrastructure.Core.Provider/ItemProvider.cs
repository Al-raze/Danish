using Infrastrucutre.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Provider
{
    public class ItemProvider : IItemProvider
    {
        IItemRepository _itemRepository;

        public ItemProvider(IItemRepository itemRepository)
        {
            this._itemRepository = itemRepository;
        }

        public List<ItemMaster> GetItems(string itemName="")
        {
            return _itemRepository.GetItems(itemName);
        }

       

        public List<ItemMaster> GetItems(out int totalCount, string itemName = "", string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0)
        {
            return _itemRepository.GetItems(out totalCount,itemName,filter,filterText, jtStartIndex, jtPageSize);
        }

        public List<ItemMaster> GetItemsWithStock(out int rowCount, string id, string filter, string filterText, int jtStartIndex, int jtPageSize)
        {
            return _itemRepository.GetItemsWithStock(out rowCount, id, filter, filterText, jtStartIndex, jtPageSize);
        }
        //____________________________________________________FBA REQUEST AND FBA REQUEST LIST _____________________________________________//
        public List<ItemMaster> GetItemsWithStocks(out int rowCount, string id, string filter, string filterText, int jtStartIndex, int jtPageSize)
        {
            return _itemRepository.GetItemsWithStock(out rowCount, id, filter, filterText, jtStartIndex, jtPageSize);
        }

        public bool AddItems(ItemMaster item) //
        {
            return _itemRepository.AddItems(item);
        }

        public bool AddFBARequest(FbaRequest item) // Fba Request
        {
            return _itemRepository.AddFBARequest(item);
        }

        public bool UpdateFBARequest(FbaRequest item, int userId)
        {
            return _itemRepository.UpdateFbaRequest(item, userId);
        }
        public bool DeleteFBARequest(int FBARequestID) //DElete FBA Request 
        {
            return _itemRepository.DeleteFBARequest(FBARequestID);
        }

        public ItemMaster GetItemsByID(int itemID)
        {
            return _itemRepository.GetItemsByID(itemID);
        }

        public ItemMaster GetsItemsByID(int itemID)
        {
            return _itemRepository.GetsItemsByID(itemID);
        }

        public bool UpdateItems(ItemMaster item, int userId)
        {
            return _itemRepository.UpdateItems(item, userId);
        }

        public bool DeleteItem(int itemID)
        {
            return _itemRepository.DeleteItem(itemID);
        }

        public List<ItemHistory> GetItemHistory(int id)
        {
            return _itemRepository.GetItemHistory(id);
        }

        public List<Supplier> GetSuppliers()
        {
            return _itemRepository.GetSuppliers();
        }

        public List<ItemCategory> GetCategories()
        {
            return _itemRepository.GetCategories();
        }

        public List<ItemManufacturer> GetManufacturers()
        {
            return _itemRepository.GetManufacturers();
        }

        public List<ItemColor> GetColors()
        {
            return _itemRepository.GetColors();
        }

        public List<ItemLocation> GetLocations()
        {
            return _itemRepository.GetLocations();
        }


        public bool AddItemCategory(ItemCategory category)
        {
            return _itemRepository.AddItemCategory(category);
        }


        public bool UpdateItemCategory(ItemCategory category)
        {
            return _itemRepository.UpdateItemCategory(category);
        }


        public ItemCategory GetCategoryByID(int categoryID)
        {
            return _itemRepository.GetCategoryByID(categoryID);
        }


        public ItemColor GetItemColorByID(int colorID)
        {
            return _itemRepository.GetItemColorByID(colorID);

        }

        public bool AddItemColor(ItemColor color)
        {
            return _itemRepository.AddItemColor(color);

        }

        public bool UpdateItemColor(ItemColor color)
        {
            return _itemRepository.UpdateItemColor(color);

        }




        public ItemLocation GetItemLocationByID(int locationID)
        {
            return _itemRepository.GetItemLocationByID(locationID);
        }

        public bool AddItemLocation(ItemLocation location)
        {
            return _itemRepository.AddItemLocation(location);
        }

        public bool UpdateItemLocation(ItemLocation location)
        {
            return _itemRepository.UpdateItemLocation(location);
        }

        public ItemManufacturer GetItemManufacturerByID(int manufacturerID)
        {
            return _itemRepository.GetItemManufacturerByID(manufacturerID);
        }

        public bool AddItemManufacturer(ItemManufacturer manufacturer)
        {
            return _itemRepository.AddItemManufacturer(manufacturer);
        }

        public bool UpdateItemManufacturer(ItemManufacturer manufacturer)
        {
            return _itemRepository.UpdateItemManufacturer(manufacturer);
        }

        public ItemMaster GetItemByID(int id)
        {
            return _itemRepository.GetItemByID(id);
        }

        public bool AddAmazonItemCategory(AmazonCategory category)
        {
            return _itemRepository.AddAmazonItemCategory(category);
        }

        public bool AddEbayItemCategory(EbayCategory category)
        {
            return _itemRepository.AddEbayItemCategory(category);
        }

        public List<EbayCategory> GetEbayCategories()
        {
            return _itemRepository.GetEbayCategories();
        }

        public EbayCategory GetEbayCategoryByID(int categoryID)
        {
            return _itemRepository.GetEbayCategoryByID(categoryID);
        }

        public bool UpdateEbayItemCategory(EbayCategory category)
        {
            return _itemRepository.UpdateEbayItemCategory(category);
        }

        public List<AmazonCategory> GetAmazonCategories()
        {
            return _itemRepository.GetAmazonCategories();
        }

        public AmazonCategory GetAmazonCategoryByID(int categoryID)
        {
            return _itemRepository.GetAmazonCategoryByID(categoryID);
        }

        public bool UpdateAmazonItemCategory(AmazonCategory category)
        {
            return _itemRepository.UpdateAmazonItemCategory(category);
        }

        public List<ItemMaster> GetItemNames()
        {
            return _itemRepository.GetItemNames();
        }

        public ItemMaster GetItemInfoByID(int itemID)
        {
            return _itemRepository.GetItemInfoByID(itemID);
        }

        public ItemMaster GetsItemsInfoByID(int itemID)
        {
            return _itemRepository.GetsItemsByID(itemID);
        }

        public List<ItemMaster> GetItemsBySupplier(int supplierID)
        {
            return _itemRepository.GetItemsBySupplier(supplierID);
        }


        public bool UpdateItemStock(Infrastrucutre.Core.Models.ViewModels.OrderRequest items)
        {
            return _itemRepository.UpdateItemStock(items);
        }

        public List<BulkInsert> CreateListings(int[] items)
        {
            return _itemRepository.CreateListings(items);
        }

        public List<ReportSummary> GetSalesReport(string fromDate, string toDate)
        {
            return _itemRepository.GetSalesReport(fromDate, toDate);
        }

        /* -------------------------------------------------------30-11-2020- Danish Bagwan ---------------------------------*/
        public List<SKUReport> GetSKUReport(string fromDate, string toDate,string Asin, string SKU, string ItemMasterID, string SellerID) // Retrive SKU DATA
        {
            return _itemRepository.GetSKUReport(fromDate, toDate, Asin,SKU, ItemMasterID,SellerID);
        }

        public List<Sku_Report> Get_SKUReport(string fromDate, string toDate, string Asin,string SKU, string SellerID) // Retrive SKU Summary
        {
            return _itemRepository.Get_SKUReport(fromDate, toDate, Asin,SKU,SellerID);
        }

        public List<Asin_Report> Get_AsinReport(string fromDate, string toDate, string Asin , string ItemMasterID) // Retrive Asin Data
        {
            return _itemRepository.Get_AsinReport(fromDate, toDate, Asin, ItemMasterID);
        }

        public List<AsinSub_Report> Get_AsinSubReport(string fromDate, string toDate, string Asin) // Retrive Item Data
        {
            return _itemRepository.Get_AsinSubReport(fromDate, toDate, Asin);
        }

        public List<ItemSummy> Get_ReportItemSummary(string fromDate, string toDate, string Asin) // Retrive Item Summary Data
        {
            return _itemRepository.Get_ReportItemSummary(fromDate, toDate, Asin);
        }
        
        public List<ItemMaster> GetUserItemByID(int UserID) // Get User Item List 
        {
            return _itemRepository.GetUserItemByID(UserID);
        }
        public List<ItemMaster> GetFNSKUListByID(string SKU) // Get FNSKU List By Item
        {
            return _itemRepository.GetFNSKUListById(SKU);
        }

        public List<ItemMaster> GetAsinByID(int ItemMasterID) // Get Asin
        {
            return _itemRepository.GetAsinByID(ItemMasterID);
        }

        public List<ItemMaster> GetSKUByID(string ListingItemNo) // Get SKU BY Asin
        {
            return _itemRepository.GetSKUByID(ListingItemNo);
        }

        public List<ItemMaster> GetItemsListByID(int SellerID) // Get SKU BY Asin
        {
            return _itemRepository.GetItemsListByID(SellerID);
        }

        public List<GroupData> Get_GroupByReport(string fromDate, string toDate, string UserId) // Retrive Group Data 02-12-2021
        {
            return _itemRepository.Get_GroupByReport(fromDate, toDate, UserId);
        }

        public List<GroupData> Get_GroupSummary(string fromDate, string toDate, string UserId) // Retrive Group Item Summary Data 02-12-2021
        {
            return _itemRepository.Get_GroupSummary(fromDate, toDate, UserId);
        }

        public List<GroupData> Get_SumGroupReport(string fromDate, string toDate, string UserId) // Retrive Group Total Data 02-12-2021
        {
            return _itemRepository.Get_SumGroupReport(fromDate, toDate, UserId);
        }

        /* --------------------------------------------------------30-11-2020- Danish Bagwan---------------------------------*/

        public List<PostalCarrier> GetPostage() // Load Postage Carrier Name 05-02-2021
        {
            return _itemRepository.GetPostage();
        }

        public List<FBALocations> InventoryLocation() // Load Inventory Location 18-02-2021
        {
            return _itemRepository.InventoryLocation();
        }

        public bool QuickUpdate(ItemMaster item)
        {
            return _itemRepository.QuickUpdate(item);
        }

        public StockSourceList GetStockSource()
        {
            return _itemRepository.GetStockSource();
        }

        public StockOut AddStockOut(StockOut option)
        {
            return _itemRepository.AddStockOut(option);
        }

        public StockView UpdateStock(ItemStock item)
        {
            return _itemRepository.UpdateStock(item);
        }

        public List<ItemStockHistory> GetStockHistory(int id, out int rowCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            return _itemRepository.GetStockHistory(id, out rowCount,jtStartIndex,jtPageSize);
        }

        public List<StockOut> GetStockOut()
        {
            return _itemRepository.GetStockOut();
        }

        public bool UpdateStockOut(StockOut option)
        {
            return _itemRepository.UpdateStockOut(option);
        }

        public List<AutoCompleteItem> GetItemNames(string itemName = "", bool onlyItemName = false)
        {
            return _itemRepository.GetItemNames(itemName, onlyItemName);
        }

        public List<AutoCompleteItem> GetSupplierNames(string supplierName = "")
        {
            return _itemRepository.GetSupplierNames(supplierName);
        }
    }
}

