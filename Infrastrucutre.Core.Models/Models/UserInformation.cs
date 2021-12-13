using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Models
{
    [Serializable]
    public class UserInformation
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }        
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        
    }

    public class Role
    {
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int RoleID { get; set; }
        public List<RolePrivilege> RolePrivileges;        
    }

    public class RolePrivilege
    {
        public int MenuID;        
        public bool GrantAccess { get; set; }        
    }
}
