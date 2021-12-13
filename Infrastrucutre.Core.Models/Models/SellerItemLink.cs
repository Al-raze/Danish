using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Infrastrucutre.Core.Models
{
    public class SellerItemLinkMapper : ClassMapper<SellerItemLink>
    {
        public SellerItemLinkMapper()
        {
            Table("SellerItemLink");
            Map(i => i.ItemName).Ignore();
            Map(i => i.ItemCode).Ignore();
            Map(i => i.ListingChannelID).Ignore();
            Map(i => i.SellerID).Ignore();
            Map(i => i.ListingChannelName).Ignore();
            //Map(i => i.FNSKU).Ignore(); // Changes made 06-02-2021
            //Map(i => i.Postage).Ignore();// Changes made 06-02-2021
            AutoMap();
        }
    }

    public class SellerItemLink
    {
        //IM.ItemMasterID, IM.ItemCode, IM.ItemName, SA.SellerID, SA.ListingChannelID, ch.ListingChannelName, SIM.ItemLinkId, SIM.ListingItemNo, SIM.SKU, 
        //SIM.ModifiedByUser, SIM.ModifiedDate

        public string ItemName { get; set; }

        public string ItemCode { get; set; }
        public string FNSKU { get; set; }
        public string Postage { get; set; }
        public string ActQty { get; set; }

        public int ListingChannelID { get; set; }

        public string ListingChannelName { get; set; }

        public int ItemLinkId { get; set; }

        [Range(1, Int32.MaxValue)]
        public int ItemMasterID { get; set; }

        [Required]
        public string ListingItemNo { get; set; }

        [Required]
        public string LinkUrl { get; set; }

        [Required]
        public string SKU { get; set; }

        public string SellerID { get; set; }

        [Required]
        public int SellerIndex { get; set; }

        public int ModifiedByUser { get; set; }

        public DateTime ModifiedDate { get; set; }

    }
}
