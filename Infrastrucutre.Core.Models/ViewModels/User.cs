using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models.ViewModels
{
    public class User
    {
        public string UserID { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

    }
}
