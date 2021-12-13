using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class AddressImport
    {
        public string CustomerNumber { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Locality { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string TelephoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ServiceName { get; set; }
        public string ProductName { get; set; }
        public string Signatureoptional { get; set; }
        public string Weight { get; set; }
        public string Items { get; set; }
        public string ExtendedLiability { get; set; }
        public string SpecialInstructions1 { get; set; }
        public string SpecialInstructions2 { get; set; }
        public string CODAmount { get; set; }
        public string CustomerRef { get; set; }
        public string AlternativeRef { get; set; }

    }
}
