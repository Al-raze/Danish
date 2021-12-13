using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models.ViewModels
{
    public class OrderRequest
    {
        public int SupplierID { get; set; }
        public Supplier SupplierRef { get; set; }
        public string OrderText { get; set; }
        public List<RequestedItem> ItemsRequested { get; set; }

    }

    public class RequestedItem
    {
        public int ItemMasterID { get; set; }
        public ItemMaster ItemRef { get; set; }
        public int RequestedItemQuantity { get; set; }
    }
}
