using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Core.DataAccess.Interfaces;
using Infrastructure.Core.Models;
using Infrastructure.Core.Providers;
using Infrastructure.Constants;
using Infrastructure.Core.Models.ViewModels;
using AT.Core.Entities;
using AT.Core.Common;

namespace Infrastructure.Core.Providers
{
    public class CustomMembershipProvider : IMembershipProvider
    {
        IUserRepository _userRepository;

        public CustomMembershipProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<MenuItem> GetAccessibleMenuItems(int roleID)
        {

            List<int> menuAccessList = _userRepository.GetMenuItemAccessList(roleID);

            var menuItems = _userRepository.GetMenuItems();

            MenuGenerator generator = new MenuGenerator();

            return generator.GetAccessibleMenuItems(roleID, menuAccessList, menuItems);
        }


        public List<UserInformation> GetUserDetails()
        {
            return _userRepository.GetUserDetails();
        }

        public bool AddUser(UserInformation user)
        {
            return _userRepository.AddUser(user);
        }


        public bool EditUser(UserInformation user)
        {
            return _userRepository.EditUser(user);
        }


        public UserInformation FindUserByName(string userName)
        {
            return _userRepository.FindUserByName(userName);
        }


        public List<Tender> GetTenderSearchDetails(string TenderNo)
        {
            return _userRepository.GetTenderSearchDetails(TenderNo);
        }
    }

}


