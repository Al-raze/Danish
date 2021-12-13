using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
    public class Tender
    {
        public int TenderID { get; set; }

        public string TenderName { get; set; }

        public string TenderNo { get; set; }

        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public int StatusID { get; set; }

        public string StatusValue { get; set; }

        public string PurchaseDate { get; set; }

        public double PurchaseValue { get; set; }

        public string ClosingDate { get; set; }

        public int DaysRequired { get; set; }

        public double ProjectValue { get; set; }

        public double GuaranteeValue { get; set; }

        public double ProportionValue { get; set; }

        public string GuaranteeStartDate { get; set; }

        public string GuaranteeEndDate { get; set; }

        public int BankID { get; set; }

        public string BankName { get; set; }

        public int BankTypeID { get; set; }

        public int IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }


        public int FileID { get; set; }

        public string FileName { get; set; }

        public int Size { get; set; }

        public string Path { get; set; }
    }
}
