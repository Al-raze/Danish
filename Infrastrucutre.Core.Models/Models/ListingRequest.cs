using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{
    public class ListingRequest
    {
        public int RequestID { get; set; }

        public string ListingRequestNo { get; set; }

        [Display(Name = "Item Name")]
        public int ItemMasterID { get; set; }

        [Display(Name = "EBay Category")]
        public int EbayCategoryID { get; set; }

        [Required]
        public string EBayRecommendedTitle1 { get; set; }

        [Required]
        public string EBayRecommendedTitle2 { get; set; }

        [Required]
        public string EBayListingReference1 { get; set; }

        [Required]
        public string EBayListingReference2 { get; set; }

        [Display(Name = "Amazon Category")]
        public int AmazonCategoryID { get; set; }

        [Required]
        public string AmazonRecommendedTitle1 { get; set; }

        [Required]
        public string AmazonRecommendedTitle2 { get; set; }

        [Required]
        public string AmazonListingReference1 { get; set; }

        [Required]
        public string AmazonListingReference2 { get; set; }


        [Required]
        public double EBayPrice { get; set; }

        [Required]
        public double AmazonPrice { get; set; }

        [Required]
        public string ListingDescription { get; set; }

        [Required]
        public string SpecialInstructions { get; set; }

        
        public double Postage { get; set; }

        public ItemMaster Item;

    }

    public class ListingRequestFilter
    {
        public string ListingRequestNo { get; set; }

        public int RequestID { get; set; }
    }


    public class ListingStatus
    {
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }

    public class ListingChannel
    {
        public int ListingChannelID { get; set; }
        public string ListingChannelName { get; set; }
    }

    public class ListingDocument
    {
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }
        public int RequestID { get; set; }
        public int IsActive { get; set; }
    }

}
