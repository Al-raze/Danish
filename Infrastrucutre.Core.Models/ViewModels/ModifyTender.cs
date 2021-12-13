using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
    public class ModifyTender
    {
        public int TenderID { get; set; }

        public int BankID { get; set; }

       [Required(ErrorMessage = " ")]
        public int StatusID { get; set; }

        
        public string StatusValue { get; set; }

        [Required(ErrorMessage = " ")]
        public string TenderName { get; set; }

        public string TenderNo { get; set; }

        public string CompanyName { get; set; }

        public string PurchaseDate { get; set; }

        [Required(ErrorMessage = " ")]
        public double PurchaseValue { get; set; }

        public string ClosingDate { get; set; }

        [Required(ErrorMessage = " ")]
        public int DaysRequired { get; set; }

        [Required(ErrorMessage = " ")]
        public double ProjectValue { get; set; }

        [RegularExpression(@"^\d{1,18}$|(?=^.{1,19}$)^\d+\.\d{1,4}$", ErrorMessage = "Invalid GuaranteeValue.")]
        public string GuaranteeValue { get; set; }

        //[Required(ErrorMessage = " ")]
        [RegularExpression(@"^\d{1,18}$|(?=^.{1,19}$)^\d+\.\d{1,4}$", ErrorMessage = "Invalid ProportionValue.")]
        public string ProportionValue { get; set; }

        
        [Required(ErrorMessage = " ")]
        public string GuaranteeStartDate { get; set; }

        [Required(ErrorMessage = " ")]
        public string GuaranteeEndDate { get; set; }

       
        public string BankName { get; set; }


        public int BankTypeID { get; set; }

        public string BankTypeName { get; set; }

        public int ModifiedBy { get; set; }
    }

    public class Color
    {
        [Required(ErrorMessage="")]
        public int ColorID { get; set; }

        public string ColorName { get; set; }
    }
}
