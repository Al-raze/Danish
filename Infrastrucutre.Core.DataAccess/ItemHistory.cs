using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class ItemHistoryMapper : ClassMapper<ItemHistory>
    {
        public ItemHistoryMapper()
        {
            Table("ItemMaster_History");
            Map(i => i.SupplierName).Ignore();
            Map(i => i.TotalCost).Ignore();
            Map(i => i.ManufacturerName).Ignore();
            Map(i => i.CategoryName).Ignore();
            Map(i => i.ActiveListings).Ignore();
            Map(i => i.CBM3).Ignore();
            Map(i => i.UserName).Ignore();
            Map(i => i.ItemMasterID).Column("ItemMasterID");
            Map(i => i.ItemCategoryID).Column("ItemCategoryID");
            Map(i => i.ItemManufacturerID).Column("ItemManufacturerID");
            Map(i => i.ItemColorID).Column("ItemColorID");
            Map(i => i.LocationID).Column("LocationID");
            Map(i => i.SupplierID).Column("SupplierID");

        //public int ItemCategoryID { get; set; }

        //public int ItemManufacturerID { get; set; }

        //public int ItemColorID { get; set; }

        //public int LocationID { get; set; }

            AutoMap();
        }
    }

    public class ItemHistory
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
                return Math.Round(Length * Width * Height / 1000000 * 100) / 100;
            }
        }


        public string UserName { get; set; }

        [Required]
        public string BarCode { get; set; }

        [Required]
        public string FNSKU { get; set; }

        public string Notes { get; set; }

        
        public int SupplierID { get; set; }

        public string SupplierName { get; set; }

        public int ItemCategoryID { get; set; }

        public int ItemManufacturerID { get; set; }

        public int ItemColorID { get; set; }

        public int LocationID { get; set; }

        public int StockUnits { get; set; }

        public double MasterCartonQty { get; set; }

        public int ReOrderLevel { get; set; }

        public DateTime? DateOfSubmission { get; set; }

        public DateTime? ModifiedDate
        {
            get;
            set;
        }

        public int ModifiedByUser { get; set; }
        

        public string ManufacturerName { get; set; }
        public string CategoryName { get; set; }
        public int ActiveListings { get; set; }
    }
}
