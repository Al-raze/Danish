using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
     public class Bank
    {
        public int BankID { get; set; }
        public string BankName { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
    }
}
