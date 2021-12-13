using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
    public class Settings
    {

        public int CompanyID { get; set; }

        [Required(ErrorMessage = " ")]
        public string CompanyName { get; set; }


        public string BankID { get; set; }

        [Required(ErrorMessage = " ")]
        public string BankName { get; set; }


        public int RivalID { get; set; }

        [Required(ErrorMessage = " ")]
        public int RivalMasterID { get; set; }

        public string TenderName { get; set; }

        public int TenderID { get; set; }

        [Required(ErrorMessage = " ")]
        public Double ProjectValue { get; set; }

        [Required(ErrorMessage = " ")]
        [Range(0, 100, ErrorMessage = "IncrementValue must between 0 and 100")]
        public double IncrementValue { get; set; }

        [Required(ErrorMessage = " ")]
        [Range(0, 100, ErrorMessage = "Discount must between 0 and 100")]
        public double Discount { get; set; }

        [Required(ErrorMessage = " ")]
        public double PurchaseValue { get; set; }

        //[Required(ErrorMessage = " ")]
        public string Note { get; set; }

        // public int RivalMasterID { get; set; }
        [Required(ErrorMessage = " ")]
        public string RivalName { get; set; }

        [Required(ErrorMessage = " ")]
        public string Address1 { get; set; }

        [Required(ErrorMessage = " ")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = " ")]
        public string Address3 { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }

        public string FileName { get; set; }

        public int GID { get; set; }

        [Required(ErrorMessage = " ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " ")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = " ")]
        public string Del { get; set; }
    }
}
