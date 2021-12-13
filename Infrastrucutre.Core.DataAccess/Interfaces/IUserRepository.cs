
using AT.Core.Entities;
using Infrastructure.Core.Models;
using Infrastructure.Core.Models.ViewModels;
using System.Collections.Generic;


namespace Infrastructure.Core.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        List<MenuItem> GetMenuItems();
        UserInformation FindUserByName(string userName);
        List<int> GetMenuItemAccessList(int roleID);
        List<UserInformation> GetUserDetails();
        bool AddUser(UserInformation user);
        bool EditUser(UserInformation user);
        List<Tender> GetTenderSearchDetails(string TenderNo);
    }
}
