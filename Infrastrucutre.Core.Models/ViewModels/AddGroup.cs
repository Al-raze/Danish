using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucutre.Core.Models.ViewModels
{
    public class AddGroup
    {
        public int GID { get; set; }

        [Required(ErrorMessage = " ")]
        public int UserID { get; set; }

        [Required(ErrorMessage = " ")]
        public int ItemMasterID { get; set; }

        [Required(ErrorMessage = " ")]
        public string Del { get; set; }
        public int[] AccessList { get; set; }

    }
}
