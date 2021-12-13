using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.Models.ViewModels
{
    public class AddRole
    {
        public string RoleName { get; set; }
        public int RoleID { get; set; }
        public List<int> RoleMenuPrivileges;
    }
}
