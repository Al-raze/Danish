using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
    public class AddUser
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = " ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " ")]
        public string Password { get; set; }

        //[Required(ErrorMessage = " ")]
        //public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = " ")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ")]
        public string Phone { get; set; }

        public int CreatedBy { get; set; }
        
        [Required(ErrorMessage = " ")]
        public int[] AccessList { get; set; }
    }

   public class AddGroup
    {
        public int GID { get; set; }

        [Required(ErrorMessage = " ")]
        public int UserID { get; set; }

        [Required(ErrorMessage = " ")]
        public int ItemMasterID { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = " ")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = " ")]
        public string Del { get; set; }
        [Required(ErrorMessage = " ")]
        public int[] AccessList { get; set; }

    }

   public class UpdateGroup
    {
        public int GID { get; set; }

        [Required(ErrorMessage = " ")]
        public int UserID { get; set; }

        [Required(ErrorMessage = " ")]
        public int ItemMasterID { get; set; }
        public string UserName { get; set; }

        [Required(ErrorMessage = " ")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = " ")]
        public string Del { get; set; }
        [Required(ErrorMessage = " ")]
        public int[] AccessList { get; set; }

    }
}
