using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class SupplierOrder
    {
        public int ItemMasterID { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double ItemCost { get; set; }

        [Display(Name = "Supplier")]
        [Required]
        public int SupplierID { get; set; }

        public string SupplierName { get; set; }

        public int CurrentStock { get; set; }

        public int UnitPerCarton { get; set; }

        public int MasterCartonQty { get; set; }

        public int CartonQty { get; set; }

        public float CBM { get; set; }

        public float TotalCBM { get; set; }

        public float TotalOrderUnit { get; set; }

        public float FOBPricePerItem { get; set; }

        public float TotalFOBPrice { get; set; }

        public string Notes { get; set; }

        //Carton QTY	Total Order Unit	Total CBM	fob price/item	Total FOB Price	Notes


    }
}
