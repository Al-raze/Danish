using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Infrastrucutre.Core.Models
{
    public class BulkInsert
    {
        [Required]
        public int ItemMasterID { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public double EBayPrice { get; set; }
        [Required]
        public int EBayCategoryID { get; set; }
        [Required]
        public int AmazonCategoryID { get; set; }
        [Required]
        public double AmazonPrice { get; set; }
        [Required]
        public string ListingStatus { get; set; }
        [Required]
        public string ListingItemPrice { get; set; }
        [Required]
        public string ListingItemNo { get; set; }
        [Required]
        public string ItemMiniTitle { get; set; }
        [Required]
        public string ListingLink { get; set; }
        [Required]
        public int ListingChannelID { get; set; }        
        
    }
}
