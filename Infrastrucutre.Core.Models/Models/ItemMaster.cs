using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class ItemMasterMapper : ClassMapper<ItemMaster>
    {
        public ItemMasterMapper()
        {
            Table("ItemMasters");
            Map(i => i.SupplierName).Ignore();
            Map(i => i.TotalCost).Ignore();
            Map(i => i.ManufacturerName).Ignore();
            Map(i => i.CategoryName).Ignore();
            Map(i => i.ActiveListings).Ignore();
            Map(i => i.CBM3).Ignore();
            Map(i => i.StockIn).Ignore();
            Map(i => i.StockOut).Ignore();
            Map(i => i.CurrentStock).Ignore();
            Map(i => i.SKU).Ignore();
            Map(i => i.UserID).Ignore();
            Map(i => i.ListingItemNo).Ignore();
            Map(i =>i.SellerIndex).Ignore();
            Map(i => i.UKSold7Days).Ignore();
            Map(i => i.EUSold7Days).Ignore();
            Map(i => i.USASold7Days).Ignore();
            Map(i => i.UKSold30Days).Ignore();
            Map(i => i.EUSold30Days).Ignore();
            Map(i => i.USASold30Days).Ignore();
            Map(i => i.UKWarehouse).Ignore();
            Map(i => i.EUWarehouse).Ignore();
            Map(i => i.USAWarehouse).Ignore();
            Map(i => i.Dimension1).Ignore();
            Map(i => i.FBARootID).Ignore();
        AutoMap();
        }
    }

    public class ItemStockMapper : ClassMapper<ItemStock>
    {
        public ItemStockMapper()
        {
            Table("ItemStock");            
            AutoMap();
        }
    }

    public class StockViewMapper : ClassMapper<StockView>
    {
        public StockViewMapper()
        {
            Table("StockView");
            Map(i => i.IsUpdated).Ignore();
            AutoMap();
        }
    }

    public class StockView
    {
        public int ItemMasterID { get; set; }
        public int StockIn { get; set; }
        public int StockOut { get; set; }
        public int CurrentStock { get; set; }
        public bool IsUpdated { get; set; }
    }

    public class ItemStockHistory
    {
        public int StockIn { get; set; }

        public string SupplierName { get; set; }

        public string Source { get; set; }

        public int StockOut { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedDateString
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", this.CreatedDate);
            }
        }

        public string Notes;
        public string ItemCost {
            get;set;
        }
    }

    public class ItemStock
    {
        public int StockId { get; set; }
       // public string ItemName { get; set; }

        public int ItemMasterID { get; set; }

        public int StockIn { get; set; }

        public int StockOut { get; set; }

        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public int InSource { get; set; }

        public int OutSource { get; set; }

        public string ListingItemNo { get; set; }

        public string SKU { get; set; }

        public string OrderReferenceNo { get; set; }

        public int SellerIndex { get; set; }

        public string SellerItemName { get; set; }

        public int FBARootID { get; set; } // Inventory Location 18-02-2021
    }

    public class ItemDetails { //For Item Load List by Seller 

        //-----------For Get Method------------//
        public int StockId { get; set; }
        public int FBARequestID { get; set; }
        public string ItemName { get; set; }
        public string Dimension { get; set; }
        public string ItemWeight { get; set; }
        public double CartonQty{ get; set; }
        public string ItemCode { get; set; }
        public double ItemCost { get; set; }
        public int ItemMasterID { get; set; }
        public double MasterCartonQty { get; set; }
        public int UserID { get; set; }
        public int StockIn { get; set; }
        public int StockOut { get; set; }
        public string ListingItemNo { get; set; }
        public string SKU { get; set; }
        public string FNSKU { get; set; }
        public string OrderReferenceNo { get; set; }
        public int SellerIndex { get; set; }
        public string SellerItemName { get; set; }
        public int FBARootID { get; set; }// Inventory Location 18-02-2021

        public double UKSold7Days { get; set; }
        public double EUSold7Days { get; set; }
        public double USASold7Days { get; set; }

        public double UKSold30Days { get; set; }
        public double EUSold30Days { get; set; }
        public double USASold30Days { get; set;}
        public double UKWarehouse { get; set; }
        public double EUWarehouse { get; set; }
        public double USAWarehouse { get; set; }
        public double UKFbaStock { get; set; }



    }

    public class FBALocations
    {
        public int FBARootID { get; set; }
        public string FBARoot { get; set; }
        
    }

    public class StockSource
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class StockSourceList
    {
        public List<StockSource> Purchase { get; set; }
        public List<StockSource> Sale { get; set; }
    }

    public class StockOutMapper : ClassMapper<StockOut>
    {
        public StockOutMapper()
        {
            Table("StockOut");            
            AutoMap();
        }
    }

    public class StockOut
    {
        public int Id { get; set; }

        public string Source { get; set; }
    }

    public class ItemMaster
    {
        public int ItemMasterID { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Dimension { get; set; }

        [Required]
        public string ItemWeight { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "VAT should be in the range of 1-100")]
        public int VAT { get; set; }

        [Required]
        public double ItemCost { get; set; }

        [Required]
        public double SellingPrice { get; set; }

        [Required]
        [Display(Name = "Length")]
        public double Length { get; set; }

        [Required]
        [Display(Name = "Width")]
        public double Width { get; set; }

        [Required]
        [Display(Name = "Height")]
        public double Height { get; set; }

        public double TotalCost { get; set; }

        [Display(Name = "Lead Time")]
        [Range(1, 365)]
        public int LeadTime { get; set; }

        [Display(Name = "CBM3")]
        public double CBM3
        {
            get
            {
                return Math.Round(Length * Width * Height / 1000000 * 10000) / 10000;
            }
        }


        [Required]
        public string BarCode { get; set; }

        [Required]
        public string FNSKU { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Supplier")]
        [Required]
        public int SupplierID { get; set; }

        public string SupplierName { get; set; }

        [Display(Name = "Category")]
        [Required]
        public int ItemCategoryID { get; set; }

        [Display(Name = "Manufacturer")]
        [Required]
        public int ItemManufacturerID { get; set; }

        [Display(Name = "Color")]
        [Required]
        public int ItemColorID { get; set; }

        [Display(Name = "Location")]
        [Required]
        public int LocationID { get; set; }

        [Required]
        [Display(Name = "QTY in Master Carton")]
        public double MasterCartonQty { get; set; }

        [Display(Name = "Stock Reorder Level")]
        [Required]
        public int ReOrderLevel { get; set; }

        [Display(Name = "Submission Date")]
        [Required]
        public DateTime? DateOfSubmission { get; set; }

        public DateTime? ModifiedDate
        {
            get
            {
                return DateTime.Now;
            }
        }
        [Display(Name = "Carton Qty")]
        [Required]
        public decimal CartonQty
        {
            get;set;
        }

        public string ManufacturerName { get; set; }
        public string CategoryName { get; set; }
        public int ActiveListings { get; set; }
        public int StockIn { get; set; }
        public int StockOut { get; set; }
        public int CurrentStock { get; set; }

        //____________________DISPLAY ITEMS____________________//
        public string SKU { get; set; }
        public string ListingItemNo { get; set; }
        public int SellerIndex { get; set; }
        public int UserID { get; set; }

        public double UKSold7Days { get; set; }
        public double EUSold7Days { get; set; }
        public double USASold7Days { get; set; }

        public double UKSold30Days { get; set; }
        public double EUSold30Days { get; set; }
        public double USASold30Days { get; set; }
        public double UKWarehouse { get; set; }
        public double EUWarehouse { get; set; }
        public double USAWarehouse { get; set; }
        public string Dimension1 { get; set; }

        public int FBARootID { get; set; } //ItemStock

    }

    public class UserInformation
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }

    }

    public class InventoryLocation
    {
        public string FBARootID { get; set; }
        public string FBARoot { get; set; }
        public string Del { get; set; }
    }
}
