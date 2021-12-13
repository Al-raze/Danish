using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class GetAsinByItem
    {
        public int StockId { get; set; }

        public int ItemMasterID { get; set; }

        public string ListingItemNo { get; set; }

        public string SKU { get; set; }

        public string OrderReferenceNo { get; set; }

        public int SellerIndex { get; set; }


    }
}
