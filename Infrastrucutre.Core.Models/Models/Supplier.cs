using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Infrastrucutre.Core.Models
{
    public class Supplier
    {
        [Required]
        public int SupplierID { get; set; }

        [Required]
        public string SupplierName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", ErrorMessage = "Invalid e-mail.")]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string SalesRepName { get; set; }

        [Required]
        public string WebAddress { get; set; }

        [Required]
        public string BankDetails { get; set; }

    }
}
