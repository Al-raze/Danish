using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Core.Models
{
    public class ChangePassword
    {
        public int UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "Minimum six characters")]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
