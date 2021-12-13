using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
   public class Rival
    {

       public int RivalID { get; set; }
        public int RivalMasterID { get; set; }
        public int TenderID { get; set; }
        public string TenderName { get; set; }
        public double ProjectValue { get; set; }
        public double IncrementValue { get; set; }
        public double Discount { get; set; }
        public double PurchaseValue { get; set; }
        public string Note { get; set; }
        public string RivalName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int CompanyID { get; set; }
        public int StatusID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public string ColorCode { get; set; }
    }
}
