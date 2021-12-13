using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperExtensions.Mapper;
using System.ComponentModel.DataAnnotations;

namespace Infrastrucutre.Core.Models
{
    public class SellerAccountMapper : ClassMapper<SellerAccount>
    {
        public SellerAccountMapper()
        {
            Table("SellerAccounts");
            Map(i => i.SellerIndex).Key(KeyType.Identity);
            AutoMap();
        }
    }


    public class SellerAccount
    {
        public string SellerID { get; set; }
        public int ListingChannelID { get; set; }
        [Required]
        public string AuthenticationToken { get; set; }
        public string ChannelName { get; set; }
        [Required]
        public string AccessKey { get; set; }
        [Required]
        public string MarketPlaceId { get; set; }
        [Required]
        public string SellerIdKey { get; set; }
        public int SellerIndex { get; set; }
        public bool Synchronize { get; set; }
    }
}
