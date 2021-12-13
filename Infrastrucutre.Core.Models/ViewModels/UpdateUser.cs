using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
   public class UpdateUser
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = " ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " ")]
        public string Password { get; set; }

        [Required(ErrorMessage = " ")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ")]
        public string Phone { get; set; }

        public int ModifiedBy { get; set; }

        public int[] AccessList { get; set; }

    }
}
