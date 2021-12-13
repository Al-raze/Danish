using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models
{

    public class OrderItemMapper : ClassMapper<OrderItem>
    {
        public OrderItemMapper()
        {
            Table("OrderItems");
            Map(i => i.OrderID).Column("OrderID");
            Map(i => i.ListingLink).Ignore();
            Map(i => i.ListingRequestNo).Ignore();
            Map(i => i.OrderReferenceNo).Ignore();
            Map(i => i.RequestID).Ignore();
            Map(i => i.ChannelName).Ignore();
            Map(i => i.ImageUrl).Ignore();
            Map(i => i.StockUnits).Ignore();
            Map(i => i.ItemName).Ignore();
            Map(i => i.ItemCode).Ignore();
            AutoMap();
        }
    }


    public class OrderItem
    {
        public int OrderLineItemID { get; set; }
        public int OrderID { get; set; }
        public string ItemID { get; set; }
        public string ItemTitle { get; set; }
        public int Quantity { get; set; }
        public double ShippingCost { get; set; }
        public double TransactionPrice { get; set; }
        public string ListingLink { get; set; }
        public string ListingRequestNo { get; set; }
        public string OrderReferenceNo { get; set; }
        public int RequestID { get; set; }
        public string ChannelName { get; set; }
        public string ImageUrl { get; set; }
        public int StockUnits { get; set; }
        public string AdditionalInfo { get; set; }
        public string SKUItemID { get; set; }


        public string ItemName { get; set; }
        public string ItemCode { get; set; }
    }
}
