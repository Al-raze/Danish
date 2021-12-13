using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class OrderMapper : ClassMapper<Order>
    {
        public OrderMapper()
        {
            Table("Orders");
            Map(i => i.OrderItems).Ignore();
            Map(i => i.OrderDateString).Ignore();
            Map(i => i.IsDispatched).Ignore();
            Map(i => i.PostalCarrierName).Ignore();
            Map(i => i.Weight).Ignore();
            Map(i => i.Price).Ignore();
            Map(i => i.CarrierImage).Ignore();
            Map(i => i.FirstItemID).Ignore();
            Map(i => i.AccountNumber).Ignore();
            Map(i => i.Code).Ignore();
            Map(i => i.Channel).Ignore();
            Map(i => i.ProceedDate).Ignore();
            Map(i => i.ProceedDateString).Ignore(); 
            AutoMap();
        }
    }

    public class FbaMapper : ClassMapper<FbaRequest>
    {
        public FbaMapper()
        {
            Table("FbaRequest");
            Map(i=>i.CartonQty).Ignore();
            Map(i => i.UKWarehouse).Ignore();
            Map(i => i.Dimension1).Ignore();
            Map(i => i.FBARoot).Ignore();
            Map(i => i.ItemName).Ignore();
            Map(i => i.FNSKU).Ignore();
            Map(i => i.ItemCode).Ignore();
            Map(i => i.MasterCartonQty).Ignore();
            Map(i => i.UserName).Ignore();
            Map(i => i.SendingDate).Ignore();
            Map(i => i.Status).Ignore();
            Map(i => i.ShipmentID).Ignore();
            Map(i => i.FBASendingDate).Ignore();
            Map(i => i.RDate).Ignore();
            Map(i => i.FBABoxQty).Ignore();
            Map(i => i.FinalQty).Ignore();
            Map(i => i.SellerID).Ignore();
            //Map(i => i.Date).Ignore();
            AutoMap();
        }
    }

    public class OrderState
    {
        public string OrderStatus { get; set; }
    }

    public class Order
    {
        [Key] 
        public int OrderID { get; set; }
        public string BuyerUserID { get; set; }
        public string Name { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string CityName { get; set; }
        public string StateOrProvince { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public double AmountPaid { get; set; }
        public string OrderReferenceNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Channel { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string OrderStatus { get; set; }
        public string SellerID { get; set; }
        public string CheckoutState { get; set; }
        public DateTime ?ShippedDate { get; set; }
        public string SalesOrderNumber { get; set; }
        public string PostalCarrierName { get; set; }
        public string Weight { get; set; }
        public string Price { get; set; }
        public int PostalCarrierID { get; set; }
        public string AccountNumber { get; set; }
        public string Code { get; set; }
        public string CarrierImage { get; set; }
        public string Country { get; set; }
        public int SellerIndex { get; set; }
        public string OrderType { get; set; }
        //Added only for Sorting
        public string FirstItemID { get; set; }
        public DateTime? ProceedDate { get; set; } 
        public string AddiotionalNotes { get; set; }
        public string CarrierRef { get; set; }
        public string IsDispatched
        {
            get
            {
                return ShippedDate.HasValue ? "Complete":"Pending";
            }
        }

        public string OrderDateString
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", this.OrderDate);
            }
        }
        public string ProceedDateString
        {
            get
            {
                return string.Format("{0:dd/MM/yyyy}", this.ProceedDate);
            }
        }

        public bool IsActive { get; set; }
    }

    public class FbaRequest //01/03/2021 FBA REQUEST FORM MODEL DATA
    {
        //[Key]
        public int FBARequestID { get; set; }
        [Required]
        public string Date {
            get;set;
        }

        public string RDate
        {
            get
            {
                var SendingDate = Convert.ToDateTime(Date).ToString("dd/MM/yyyy");
                return SendingDate.ToString(); 
            }
        }
        public int UserID { get; set; }
        public int SellerIndex { get; set; }
        public int ItemMasterID { get; set; }
        public string ListingItemNo { get; set; }
        public string SKU { get; set; }
        [Required]
        public string FNSKU { get; set; }
        [Required]
        public string LableStatus { get; set; }
        [Required]
        public string LableLink { get; set; }

        public string LastReqDate {
            get;set;
        }
        [Required]
        public string PriorityStatus { get; set; }
        [Required]
        public int FBARootID { get; set; } // Uk/EU/USA Like That FBA Root
        public string FBARoot { get; set; } // Uk/U/USA Like That FBA Root
        [Required]
        public decimal FBARecedQty { get; set; }
        [Required]
        public decimal RequestQty { get; set; }
        [Required]
        public string Comments { get; set; }
        [Required]
        public decimal UKSold7Days { get; set; }
        [Required]
        public decimal EUSold7Days { get; set; }
        [Required]
        public decimal USASold7Days { get; set; }
        [Required]
        public double UKSold30Days { get; set; }
        [Required]
        public double EUSold30Days { get; set; }
        [Required]
        public double USASold30Days { get; set; }
        [Required]
        public double UKWarehouse { get; set; }
        [Required]
        public double UKFbaStock { get; set; }
        [Required]
        //public double CartonQty { get; set; }

        public ItemDetails Item;

        //----------------------Display Item----------------//
        public string SellerID { get; set; }
        public string CartonQty { get; set; }
        public string ItemName { get; set; }
        public string Dimension1 { get; set; }
        public string ItemCode { get; set; }
        public double MasterCartonQty { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public string SendingDate { get;set;}
        public string FBASendingDate
        { 
            get {
                var Date = Convert.ToDateTime(SendingDate).ToString("dd/MM/yyyy");
                return Date.ToString();
            }
        }
        public string ShipmentID { get; set; }
        public double FBABoxQty { get; set; } //Fba Sorted Form
        public double FinalQty { get; set; }//Fba Sorted Form
        //_________________________________________________//
    }

    public class ShipmentDetails
    {
        public int ShipID { get; set; }
        public string ShipmentID { get; set; }
        public int FBARequestID { get; set; }

       public string ProceedDate { get; set; }
        public string Date
        { 
            get
            {
                var SendingDate = Convert.ToDateTime(ProceedDate).ToString("dd/MM/yyyy");
                return SendingDate.ToString();
            }
        }
        public string ShipmentMethod { get; set; }
        public string Destination { get; set; }
        public string SKU { get; set; }
        public string TotalFNSKU { get; set; }
        public string TotalBoxes { get; set; }
        public string TotalQty { get; set; }
        public string SellerID { get; set; }
        public string FBARoot { get; set; }
    }

}
