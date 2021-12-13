using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.TypeConversion;
using CsvHelper.Configuration;

namespace Inventory.Synchronizer
{
   public class OrderFile
    {
        public string OrderId { get; set; }
    }

   sealed class OrderFileMap : CsvClassMap<OrderFile>
   {
       public OrderFileMap()
       {
           Map(m => m.OrderId).Name("order-id");          
       }
   }
  
}
