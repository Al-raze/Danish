using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;

namespace Infrastrucutre.Core.Models
{
    //public class ReportMapper : ClassMapper<Report>
    //{
    //    public ReportMapper(){
    //        Table("Reports");
    //    }
    //}



    public class Report
    {
        public int ReportID { get; set; }

        public string ReportName { get; set; }

        public string ReportQuery { get; set; }

        public bool IsActive { get; set; }
    }

    public class SKUReport //SKU Report DATA 
    {

        public int ItemMasterID { get; set; }
        public string ItemID { get; set; }
        public string SKUItemID { get; set; }
        public string OrderReferenceNo { get; set; }
        public string ItemName { get; set; }
        public string ODate { get; set; }
        public string OrderDate {
            get
            {
                var OrDate = Convert.ToDateTime(ODate).ToString("dd/MM/yyyy");
                return OrDate.ToString();
            }
        }
        public string SellerID { get; set; }
        public string Country { get; set; }
        public int Quantity { get; set; }
        public string ItemCost { get; set; }
        public string TotalCost { get; set; }
        public Decimal AmountPaid { get; set; }
        public string DCharg {
            get {
                return Math.Round((2.44), 2).ToString();
            }
        }
        public string FBACharge
        {
            get
            {
                var FBA = 2.49;
                var FBACharge = Math.Round(Convert.ToDouble(FBA) * (Convert.ToDouble(Quantity)), 2).ToString();
                return Math.Round(Convert.ToDouble(FBACharge), 2).ToString();
            }
        }
        public string PVatAmount // For Used Only PayableVat
        {
            get {
                var per = 0.20;
                var Amount = Math.Round(Convert.ToDouble(AmountPaid) * Convert.ToDouble(per), 2);
                return Math.Round(Convert.ToDouble(Amount), 2).ToString();
            }
        }
        public string TotalCostItemCost //For Used Only PayableVat
        {
            get
            {
                var Amount = Math.Round((Convert.ToDouble(TotalCost) - Convert.ToDouble(ItemCost)), 2).ToString();
                return Math.Round(Convert.ToDouble(Amount), 2).ToString();
            }
        }
        public string TotalCostByQuantity //TotalCost * Quantity
        {
            get
            {
                var Amount = Math.Round((Convert.ToDouble(TotalCost) * Convert.ToDouble(Quantity)), 2).ToString();
                return Math.Round(Convert.ToDouble(Amount), 2).ToString();
            }
        }
        public string PVat { //Payable Vat
            get {

                var PVat = Math.Round((Convert.ToDouble(PVatAmount)), 2).ToString();
                // Math.Round((Convert.ToDouble(PVatAmount) - Convert.ToDouble(TotalCostItemCost)),2).ToString();

                return Math.Round(Convert.ToDouble(PVat), 2).ToString();
            }
        }
        public string Promo
        {
            get {
                var per = 0.10;
                var Promo = Math.Round((Convert.ToDouble(AmountPaid)) * (Convert.ToDouble(per)), 2).ToString();
                return Promo;
            }

        }
        public string Fees
        {
            get
            {
                var per = 0.15;
                var Promo = Math.Round((Convert.ToDouble(AmountPaid)) * (Convert.ToDouble(per)), 2).ToString();
                return Promo;
            }
        }
        public string Errors
        { get
            {
                var per = 0.10;
                var Error = Math.Round((Convert.ToDouble(AmountPaid)) * (Convert.ToDouble(per)), 2).ToString();
                return Error;
            }
        }
        public string Addition //For Profit Used
        { get
            {
                var Addition = ((Convert.ToDouble(TotalCostByQuantity) + Convert.ToDouble(Fees) + Convert.ToDouble(Promo) + Convert.ToDouble(PVatAmount) + Convert.ToDouble(FBACharge) + Convert.ToDouble(Errors))).ToString();
                return Math.Round(Convert.ToDouble(Addition), 2).ToString();
            }
        }
        public string Profit //For Profit Used
        {
            get
            {
                var Profit = Math.Round((Convert.ToDouble(AmountPaid)) - (Convert.ToDouble(Addition)), 2).ToString();
                return Math.Round(Convert.ToDouble(Profit), 2).ToString();
            }
        }
        public string ProfitLevel //Profit Level
        {
            get {

                var PLevel = Math.Round(((Convert.ToDouble(Profit)) / (Convert.ToDouble(TotalCostByQuantity))) * 100).ToString();
                return Math.Round(Convert.ToDouble(PLevel), 2).ToString();
            }
        }
        public string PLevel //Profit Level
        {
            get
            {

                var PLevel = Math.Round((Convert.ToDouble(ProfitLevel)) * (Convert.ToDouble(Quantity))).ToString();
                return Math.Round(Convert.ToDouble(PLevel), 2).ToString();
            }
        }


    }

    public class Sku_Report //SKU Data Count By Country
    {
        public string ODate { get; set; }
        public string OrderDate {
            get
            {
                var OrDate = Convert.ToDateTime(ODate).ToString("dd/MM/yyyy");
                return OrDate.ToString();
            }
        }
        public string ItemID { get; set; }
        public string GB
        { get; set; }
        public string UnitedKingdom
        { get; set; }
        public string FR
        { get; set; }
        public string IT
        { get; set; }
        public string DE
        { get; set; }
        public string ES
        { get; set; }
        public string NL
        { get; set; }
        public string SE
        { get; set; }
        public string USA
        { get; set; }
        public string CA
        { get; set; }
        public string Smaya
        { get; set;
        }
        public string Quantity
        {
            get; set;
        }
        public string Salsabil
        { get; set; }
        public string Etsy
        { get; set; }
        public string CDisc
        { get; set; }

        public string Total
        {
            get
            {
                var Total = Math.Round(Convert.ToDouble(GB) + Convert.ToDouble(FR) + Convert.ToDouble(IT) + Convert.ToDouble(DE) + Convert.ToDouble(ES) + Convert.ToDouble(NL) + Convert.ToDouble(SE) + Convert.ToDouble(USA) + Convert.ToDouble(CA) + Convert.ToDouble(Smaya) + Convert.ToDouble(Salsabil) + Convert.ToDouble(Etsy) + Convert.ToDouble(CDisc)).ToString();

                return Total.ToString();
            }
        }



    }

    public class Asin_Report //Asin Data Report......
    {
        public string OrderDate { get; set; }
        public string ItemID { get; set; }
        public string GB
        { get; set; }
        public string FR
        { get; set; }
        public string IT
        { get; set; }
        public string DE
        { get; set; }
        public string ES
        { get; set; }
        public string NL
        { get; set; }
        public string SE
        { get; set; }
        public string USA
        { get; set; }
        public string CA
        { get; set; }
        public string Seller
        { get; set; }
        public string AsinNo
        { get; set; }
        public string FNSKU
        { get; set; }
        public string SalsabilANDSmaya
        { get; set; }
        public string Etsy
        { get; set; }
        public string FMethod
        { get; set; }
        public string TotalSold
        {
            get {

                var TotalSold = Math.Round(Convert.ToDouble(GB) + Convert.ToDouble(FR) + Convert.ToDouble(IT) + Convert.ToDouble(DE) + Convert.ToDouble(ES) + Convert.ToDouble(NL) + Convert.ToDouble(SE) + Convert.ToDouble(USA) + Convert.ToDouble(CA) + Convert.ToDouble(SalsabilANDSmaya)).ToString();

                return TotalSold.ToString();
            }
        }
    }

    public class AsinSub_Report //For Sub Data Report......
    {
        
        public string ODate
        {
            get; set;

        }
        public string OrderDate
        { get
            {
                var OrDate = Convert.ToDateTime(ODate).ToString("dd/MM/yyyy");
                return OrDate.ToString();
            }
        }
        public string ItemID
        { get; set; }
        public string GB
        { get; set; }
        public string FR
        { get; set; }
        public string IT
        { get; set; }
        public string DE
        { get; set; }
        public string ES
        { get; set; }
        public string NL
        { get; set; }

        public string UKNeez
        { get; set; }
        public string FRNeez
        { get; set; }
        public string ITNeez
        { get; set; }
        public string DENeez
        { get; set; }
        public string ESNeez
        { get; set; }
        public string NLNeez
        { get; set; }
        public string Samaya
        { get; set; }
        public string Salsabil
        { get; set; }

       /* public string CDisc
        { get; set; }*/
        public string Etsy
        { get; set; }

        public string TotalSold
        {
            get {
                var TotalQty = Math.Round(Convert.ToDouble(GB) + Convert.ToDouble(FR) + Convert.ToDouble(IT) + Convert.ToDouble(DE) + Convert.ToDouble(ES) + Convert.ToDouble(NL), 2).ToString();
                return TotalQty.ToString();
            }

        }
        
        
    }


    public class ItemSummy //For Item Report Summary Data ......
    {
        public string ItemName
        { get; set; }
        public string TotalAsin
        { get; set; }
        public string TotalSku
        { get; set; }
        public string TotalSold
        {
            get;
            set;
        }
        public string UKFbaStock
        { get; set; }
        public string EUFbaStock
        { get; set; }
        public string UkWareHouse
        { get; set; }
        public string EUWareHouse
        { get; set; }
        
    }



    public class ReportSummary // Item Report Data
    {
        public string ItemTitle { get; set; }
        public string ItemID { get; set; }
        public string SKUItemID { get; set; }
        public int QTY { get; set; }
        public double ItemCost { get; set; }
        public double SalePrice { get; set; }
        public double SalesCommision { get; set; }
        public string Profit
        {
            get
            {
                if (ItemCost > 0)
                {
                    return Math.Round((SalePrice + SalesCommision) - ItemCost, 2).ToString();
                }

                return "N/A";

            }

        }
        public string ProfitPercent
        {
            get
            {
                if (ItemCost > 0)
                {
                    var profit = Math.Round((SalePrice + SalesCommision) - ItemCost, 2);

                    var percent = Math.Round((profit / (SalePrice + SalesCommision) * 100), 2);

                    return percent.ToString();
                }

                return "N/A";
            }

        }
        public string ImageUrl { get; set; }

    }

    //-----------------------Group Data -------------------//

    public class GroupData //For Sub Data Report......
    {
        
                
        public string ItemName
        { get; set; }
        public string ItemID
        { get; set; }
        public string GB
        { get; set; }
        public string FR
        { get; set; }
        public string IT
        { get; set; }
        public string DE
        { get; set; }
        public string ES
        { get; set; }
        public string NL
        { get; set; }
        public string PL
        { get; set; }
        public string IE
        { get; set; }
        public string BE
        { get; set; }

        public string GBNeez
        { get; set; }
        public string FRNeez
        { get; set; }
        public string ITNeez
        { get; set; }
        public string DENeez
        { get; set; }
        public string ESNeez
        { get; set; }
        public string NLNeez
        { get; set; }
        public string PLNeez
        { get; set; }
        public string Samaya
        { get; set; }
        public string Salsabil
        { get; set; }

        /* public string CDisc
         { get; set; }*/
        public string Etsy
        { get; set; }

        public string TotalSold
        {
            get
            {
                var TotalQty = Math.Round(Convert.ToDouble(GB) + Convert.ToDouble(FR) + Convert.ToDouble(IT) + Convert.ToDouble(DE) + Convert.ToDouble(ES) + Convert.ToDouble(NL), 2).ToString();
                return TotalQty.ToString();
            }

        }

        public string totalGB
        {
            get {

                var total = 0;  //set a variable that holds our total
                if (GB == null)
                {
                    GB = "0";
                }
                for (int i = 0; i < GB.Length; i++)
                {  //loop through the array
                    total += GB[i];  //Do the math!
                }

                return total.ToString();
            }
        }

        public string ItemCount // Summery Data 
        { get; set; }

        public string GroupName // Summery Data 
        { get; set; }


    }
}
