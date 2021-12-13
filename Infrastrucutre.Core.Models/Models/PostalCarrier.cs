using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class PostalCarrier
    {
        public int PostalCarrierID { get; set; }
        public string PostalCarrierName { get; set; }
        //public string CarrierImage { get; set; }
        //public string AccountNumber { get; set; }
        //public string Code { get; set; }
        public string Dimension { get; set; } // Changes Made by Danish 28-01-2021
        public string Local { get; set; } // Changes Made by Danish 28-01-2021
        public float Weight { get; set; }// Add New Field by Danish 28-01-2021
        public float Price { get; set; }// Add New Field by Danish 28-01-2021
        public float FBAUK { get; set; }// Add New Field by Danish 28-01-2021
        public float FBADE { get; set; }// Add New Field by Danish 28-01-2021
        public float FBADE1 { get; set;}// Add New Field by Danish 28-01-2021
        public float FBAFR { get; set; }// Add New Field by Danish 28-01-2021
        public float FBAIT { get; set; }// Add New Field by Danish 28-01-2021
        public float FBAES { get; set; }// Add New Field by Danish 28-01-2021
        public float FBANL { get; set; } // Add New Field by Danish 28-01-2021
        public float FBASE { get; set; } // Add New Field by Danish 28-01-2021
        public float FBAUSA { get; set; } // Add New Field by Danish 28-01-2021
        public float FBACA { get; set; } // Add New Field by Danish 28-01-2021
        public float OTHER { get; set; } // Add New Field by Danish 28-01-2021
        public float FBAPL { get; set; } // Add New Field by Danish 15-02-2021
        public float FBAAU { get; set; } // Add New Field by Danish 15-02-2021
        public float CDisc { get; set; } // Add New Field by Danish 15-02-2021
        public float MX { get; set; } // Add New Field by Danish 15-02-2021
        public float OTHER1 { get; set; } // Add New Field by Danish 15-02-2021
        public float OTHER2 { get; set; } // Add New Field by Danish 15-02-2021



    }
}
