using Infrastructure.Core.Models;
using Infrastructure.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Provider
{
    public interface ISettingsProvider
    {
        //CompanyMaster
        List<Company> GetCompanyMaster();
        Company GetCompanyMasterByID(int companyID);
        bool AddCompanyMaster(Company company);
        bool UpdateCompanyMaster(Company company);

        //BankMaster
        List<Bank> GetBankMaster();
        Bank GetBankMasterByID(int bankID);
        bool AddBankMaster(Bank bank);
        bool UpdateBankMaster(Bank bank);

        //RivalMaster
        List<Rival> GetRivalMaster();
        Rival GetRivalMasterByID(int rivalMasterID);
        bool AddRivalMaster(Rival rival);
        bool UpdateRivalMaster(Rival rival);


        List<AddUser> GetUserDetails();
        bool AddUser(AddUser user);
        bool UpdateUser(UpdateUser user);
        bool DeleteUser(int userID);
        AddUser GetUserByID(int userID);

        //---------------Group Details 08-02-2021----------------//
        List<AddGroup> GetGroupDetails(out int totalCount,int jtStartIndex = 0, int jtPageSize = 0);
        
        bool AddGroup(AddGroup user);
        bool UpdateGroup(UpdateGroup user);
        bool DeleteGroup(int GID);
        AddGroup GetGroupByID(int GID);

        //------------------------------------------------------//

        bool UpdateSettings(string fileName, string filePath, string companyName);
        WebSetting GetWebSetting();
        
        List<Role> GetRoleDetails();
        bool CheckUserExistance(string userName);

        bool ChangePassword(ChangePassword password);
    }
}
