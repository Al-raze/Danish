using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
    public class AddTender
    {
        public int TenderID { get; set; }

        public int StatusID { get; set; }

        [Required(ErrorMessage = " ")]
        public string TenderName { get; set; }

        [Required(ErrorMessage = " ")]
        public string TenderNo { get; set; }

        [Required(ErrorMessage = " ")]
        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        [Required(ErrorMessage = " ")]
        public string PurchaseDate { get; set; }

        [Required(ErrorMessage = " ")]
        public double PurchaseValue { get; set; }

        [Required(ErrorMessage = " ")]
        public string ClosingDate { get; set; }

       
        public int DaysRequired { get; set; }

       
        public double ProjectValue { get; set; }

      
        public double GuaranteeValue { get; set; }

        public double ProportionValue { get; set; }

        public string GuaranteeStartDate { get; set; }

        public string GuaranteeEndDate { get; set; }

        [Required(ErrorMessage = " ")]
        public int BankID { get; set; }

        public string BankName { get; set; }

        [Required(ErrorMessage = " ")]
        public int BankTypeID { get; set; }

        public int IsActive { get; set; }

        public string CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public string ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }

    }
}
