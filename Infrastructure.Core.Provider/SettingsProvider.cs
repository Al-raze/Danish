using Infrastructure.Core.Models;
using Infrastructure.Core.Models.ViewModels;
using Infrastrucutre.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Provider
{
   public class SettingsProvider : ISettingsProvider
    {
       ISettingsRepository _settingsRepository;

       //default
       public SettingsProvider(ISettingsRepository settingsRepository)
       {
           _settingsRepository = settingsRepository;
       }

       //Company Master
       public List<Company> GetCompanyMaster()
       {
           return _settingsRepository.GetCompanyMaster();
       }

       public Company GetCompanyMasterByID(int companyID)
       {
           return _settingsRepository.GetCompanyMasterByID(companyID);
       }

       public bool AddCompanyMaster(Company company)
       {
           return _settingsRepository.AddCompanyMaster(company);
       }

       public bool UpdateCompanyMaster(Company company)
       {
           return _settingsRepository.UpdateCompanyMaster(company);
       }


       //Bank Master
       public List<Bank> GetBankMaster()
       {
           return _settingsRepository.GetBankMaster();
       }

       public Bank GetBankMasterByID(int bankID)
       {
           return _settingsRepository.GetBankMasterByID(bankID);
       }

       public bool AddBankMaster(Bank bank)
       {
           return _settingsRepository.AddBankMaster(bank);
       }

       public bool UpdateBankMaster(Bank bank)
       {
           return _settingsRepository.UpdateBankMaster(bank);
       }


       //Rival Master
       public List<Rival> GetRivalMaster()
       {
           return _settingsRepository.GetRivalMaster();
       }

       public Rival GetRivalMasterByID(int rivalMasterID)
       {
           return _settingsRepository.GetRivalMasterByID(rivalMasterID);
       }

       public bool AddRivalMaster(Rival rival)
       {
           return _settingsRepository.AddRivalMaster(rival);
       }

       public bool UpdateRivalMaster(Rival rival)
       {
           return _settingsRepository.UpdateRivalMaster(rival);
       }
       public List<AddUser> GetUserDetails()
       {
           return _settingsRepository.GetUserDetails();
       }


       public bool AddUser(AddUser user)
       {
           return _settingsRepository.AddUser(user);
       }

       public bool UpdateUser(UpdateUser user)
       {
           return _settingsRepository.UpdateUser(user);
       }


       public bool DeleteUser(int userID)
       {
           return _settingsRepository.DeleteUser(userID);
       }
        //-------------------------------------------Group Details -----------------------//
        public bool AddGroup(AddGroup userID)
        {
            return _settingsRepository.AddGroup(userID);
        }

        public bool UpdateGroup(UpdateGroup userID)
        {
            return _settingsRepository.UpdateGroup(userID);
        }      

        public bool DeleteGroup(int GID)
        {
            return _settingsRepository.DeleteGroup(GID);
        }
        
        public List<AddGroup> GetGroupDetails(out int totalCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            return _settingsRepository.GetGroupDetails(out totalCount, jtStartIndex, jtPageSize);
        }
        public AddGroup GetGroupByID(int GID)
        {
            return _settingsRepository.GetGroupByID(GID);
        }
        //---------------------------------------------------------------------------------//
        public AddUser GetUserByID(int userID)
       {
           return _settingsRepository.GetUserByID(userID);
       }


       public bool UpdateSettings(string fileName, string filePath, string companyName)
       {
           return _settingsRepository.UpdateSettings(fileName, filePath, companyName);
       }


       public WebSetting GetWebSetting()
       {
           return _settingsRepository.GetWebSetting();
       }

       public List<Role> GetRoleDetails()
       {
           return _settingsRepository.GetRoleDetails();
       }

       public bool ChangePassword(ChangePassword password)
       {
           return _settingsRepository.ChangePassword(password);
       }

       public bool CheckUserExistance(string userName)
       {
           return _settingsRepository.CheckUserExistance(userName);
       }
    }
}
