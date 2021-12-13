using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DapperExtensions.Mapper;

namespace Infrastrucutre.Core.Models
{
    public class ListingSubmissionMapper : ClassMapper<ListingSubmission>
    {
        public ListingSubmissionMapper()
        {
            Table("ListingSubmissions");
            Map(i => i.Request).Ignore();
            AutoMap();
        }
    }



    public class ListingSubmission
    {
        public int ListingSubmissionID { get; set; }
        [Required]
        public int ListingRequestID { get; set; }
        [Required]
        public string ListingStatus { get; set; }
        [Required]
        public double ListingItemPrice { get; set; }
        [Required]
        public string ListingItemNo { get; set; }
        [Required]
        public string ItemMiniTitle { get; set; }
        [Required]
        public string ListingLink { get; set; }
        [Required]
        public int ListingChannelID { get; set; }

        public ListingRequest Request { get; set; }
    }


    //Only for Grid Display - Not to be used for Updates / Delete
    public class Submission
    {
        public int RequestID { get; set; }
        public int ItemMasterID { get; set; }
        public int ListingSubmissionID { get; set; }
        public string ListingRequestNo { get; set; }
        public string ListingDescription { get; set; }
        public string ItemCode { get; set; }
        public string ListingItemNo { get; set; }
        public string ItemMiniTitle { get; set; }
        public double ListingItemPrice { get; set; }
        public string ListingChannelName { get; set; }
        public string ListingLink { get; set; }
        public string StatusDescription { get; set; }
    }

}
