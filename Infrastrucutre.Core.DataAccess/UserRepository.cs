using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.ConfigurationProvider;
using Dapper;
using Infrastructure.Core.Models;
using Infrastructure.Core.Models.ViewModels;
using Infrastructure.Core.DataAccess.Interfaces;
using AT.Core.Entities;
using AT.Core.Common;


namespace Infrastructure.Core.DataAccess
{
    public class UserRepository : IUserRepository
    {
        public List<Label> GetLabelForPage(string LabelName,string PageName)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                string query = string.Format("SELECT PL.LabelName, PL.LabelTextEn as LabelText, 'English' as Language, PM.PageName FROM PageLabel PL  INNER JOIN PageMaster PM ON PL.PageID = PM.PageID  WHERE PL.LabelName='{0}' AND PM.PageName='{1}'  UNION " +
                                             "SELECT PL.LabelName,PL.LabelTextAr as LabelText, 'Arabic' as Language, PM.PageName FROM PageLabel PL INNER JOIN PageMaster PM ON PL.PageID = PM.PageID  WHERE PL.LabelName='{0}' AND PM.PageName='{1}'", LabelName, PageName);

                List<Label> labelItems = connection.Query<Label>(query).ToList();

                return labelItems;
            }

        }
        
        public List<MenuItem> GetMenuItems()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string query = "SELECT MenuID,Name,ParentID,SortOrder,ActionUrl,Description " +
                                     "FROM Inventory_MenuItems ";

                List<MenuItem> menuItems = connection.Query<MenuItem>(query).ToList();

                return menuItems;
            }
        }

        public UserInformation FindUserByName(string userName)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("SELECT UserName,UserID,Password,r.RoleID,r.RoleName " +
                                     "FROM AppUsers u inner join Inventory_Roles r on u.RoleID = r.RoleID WHERE UserName='{0}'", userName);

                IEnumerable<UserInformation> userList = connection.Query<UserInformation>(query);

                return userList.FirstOrDefault();
            }
        }

        public List<int> GetMenuItemAccessList(int roleID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("Select MenuID from Inventory_RoleMenuPrivileges Where RoleID={0}", roleID);

                List<int> accessList = connection.Query<int>(query).ToList();

                return accessList;
            }

        }

        public List<UserInformation> GetUserDetails()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("SELECT * FROM AppUsers");

                IEnumerable<UserInformation> accessList = connection.Query<UserInformation>(query).ToList();

                return accessList.ToList();
            }
        }


        public bool AddUser(UserInformation user)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("INSERT INTO USERINFO (USERNAME,PHONE,LOCATION) VALUES ('{0}','{1}','{2}')",user.UserName,user.Phone,user.Location);

                int rowsAffected = connection.Execute(query);

                return rowsAffected > 0;
            }
        }


        public bool EditUser(UserInformation user)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("UPDATE USERINFO SET USERNAME = '{0}',PHONE = '{1}',LOCATION = '{2}' WHERE USERID = {3}", user.UserName, user.Phone, user.Location,user.UserID);

                int rowsAffected = connection.Execute(query);

                return rowsAffected > 0;
            }
        }


        public List<Tender> GetTenderSearchDetails(string TenderNo)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@TenderNo", TenderNo, DbType.String, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.GetTenderSearchDetails";

                IEnumerable<Tender> getSearchDetails = connection.Query<Tender>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getSearchDetails.ToList();
            }
        }

        
    }
}
