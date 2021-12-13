using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class YodelImport
    {
        public string AccountID { get; set; }
        public string TariffCode { get; set; }
        public string YourReference { get; set; }
        public string Del_Company { get; set; }
        public string Del_Address1 { get; set; }
        public string Del_Address2 { get; set; }
        public string Del_Address3 { get; set; }
        public string Del_Town { get; set; }
        public string Del_County { get; set; }
        public string Del_PostCode { get; set; }
        public string Del_Contact_1 { get; set; }
        public string Del_Phone { get; set; }
        public string Del_Email { get; set; }
        public string Pieces { get; set; }
        public string Weight { get; set; }
        public string Customs_Data { get; set; }
        public string Notes { get; set; }
    }
}
