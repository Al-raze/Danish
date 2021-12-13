using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Models.ViewModels
{
    public class WebSetting
    {
        [Required(ErrorMessage = " ")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = " ")]
        public string CompanyLogo { get; set; }


    }
}
