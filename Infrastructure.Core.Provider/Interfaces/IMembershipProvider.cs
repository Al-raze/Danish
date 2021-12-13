using Infrastructure.Core.Models.ViewModels;
using Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AT.Core.Entities;


namespace Infrastructure.Core.Providers
{
    public interface IMembershipProvider
    {
        List<MenuItem> GetAccessibleMenuItems(int roleID);
        List<UserInformation> GetUserDetails();
        UserInformation FindUserByName(string userName);
        bool AddUser(UserInformation user);
        bool EditUser(UserInformation user);
        List<Tender> GetTenderSearchDetails(string TenderNo);

    }
}
