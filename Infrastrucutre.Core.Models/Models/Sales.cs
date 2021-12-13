using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class SaleOrder
    {
        public string OrderID { get; set; }
        public string ShipmentID { get; set; }
        public string MarketplaceName { get; set; }

        public List<SaleOrderItem> OrderItems { get; set; }
    }

    public class SaleOrderItem
    {
        public string ItemCode { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public List<SalePrice> ItemPrice { get; set; }
        public List<SalePrice> ItemFees { get; set; }
    }

    public class SalePrice
    {
        public string Type { get; set; }
        public string Amount { get; set; }

    }
}
