using Dapper;
using Infrastructure.ConfigurationProvider;
using Infrastructure.Core.DataAccess;
using Infrastructure.Core.Models;
using Infrastructure.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucutre.Core.DataAccess
{
    public class SettingsRepository : ISettingsRepository
    {
        public bool ChangePassword(ChangePassword password)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                int rowsaffected = connection.Execute("UPDATE AppUsers Set Password=@password,ModifiedOn= GetDate() WHERE UserID=@userid",
                    new { password = password.NewPassword, userid = password.UserID });

                return rowsaffected > 0;
            }
        }
        public List<Company> GetCompanyMaster()
        {
            var parameters = new DynamicParameters();


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Company_GetCompany";
                IEnumerable<Company> getCompany = connection.Query<Company>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getCompany.ToList();
            }
        }

        public Company GetCompanyMasterByID(int companyID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@companyID", companyID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Company_GetCompanyByID";
                IEnumerable<Company> getCompany = connection.Query<Company>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getCompany.FirstOrDefault();
            }
        }

        public bool AddCompanyMaster(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyName", company.CompanyName, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Company_AddCompanyName";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool UpdateCompanyMaster(Company company)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyID", company.CompanyID, DbType.String, ParameterDirection.Input);
            parameters.Add("@CompanyName", company.CompanyName, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Company_UpdateCompanyMaster";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }



        public List<Bank> GetBankMaster()
        {
            var parameters = new DynamicParameters();


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Bank_GetBankName";
                IEnumerable<Bank> getBank = connection.Query<Bank>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getBank.ToList();
            }
        }

        public Bank GetBankMasterByID(int bankID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BankID", bankID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Company_GetBankMasterByID";
                IEnumerable<Bank> getBank = connection.Query<Bank>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getBank.FirstOrDefault();
            }
        }

        public bool AddBankMaster(Bank bank)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BankName", bank.BankName, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Bank_AddBankName";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool UpdateBankMaster(Bank bank)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@BankID", bank.BankID, DbType.String, ParameterDirection.Input);
            parameters.Add("@BankName", bank.BankName, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Bank_UpdateBankMaster";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }



        public List<Rival> GetRivalMaster()
        {
            var parameters = new DynamicParameters();


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Rival_GetRivalMaster";
                IEnumerable<Rival> getRival = connection.Query<Rival>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getRival.ToList();
            }
        }


        public Rival GetRivalMasterByID(int rivalMasterID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@rivalMasterID", rivalMasterID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Company_GetRivalMasterByID";
                IEnumerable<Rival> getRival = connection.Query<Rival>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getRival.FirstOrDefault();
            }
        }

        public bool AddRivalMaster(Rival rival)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RivalName", rival.RivalName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address1", rival.Address1, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address2", rival.Address2, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address3", rival.Address3, DbType.String, ParameterDirection.Input);
            // parameters.Add("@IsActive", rival.IsActive, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", rival.CreatedBy, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Rival_AddRivalMaster";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool UpdateRivalMaster(Rival rival)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RivalName", rival.RivalName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address1", rival.Address1, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address2", rival.Address2, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address3", rival.Address3, DbType.String, ParameterDirection.Input);
            // parameters.Add("@IsActive", rival.IsActive, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@RivalMasterID", rival.RivalMasterID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Rival_UpdateRivalMaster";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }




        public List<AddUser> GetUserDetails()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.InventoryUser_GetDetails";
                IEnumerable<AddUser> getRival = connection.Query<AddUser>(storedProcedure, commandType: CommandType.StoredProcedure);
                return getRival.ToList();
            }
        }

        public List<Role> GetRoleDetails()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.InventoryUser_RoleDetails";
                IEnumerable<Role> roles = connection.Query<Role>(storedProcedure, commandType: CommandType.StoredProcedure);
                return roles.ToList();
            }

        }

        public bool AddUser(AddUser user)
        {
            var parameters = new DynamicParameters();

            StringBuilder accessListXml = new StringBuilder();

            foreach (var item in user.AccessList)
            {
                accessListXml.Append("<Access>");
                accessListXml.AppendFormat("<MenuID>{0}</MenuID>", item);
                accessListXml.Append("</Access>");
            }

            parameters.Add("@UserName", user.UserName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Password", user.Password, DbType.String, ParameterDirection.Input);
            //parameters.Add("@UserID", user.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@Phone", user.Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", user.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@AccessList", accessListXml.ToString(), DbType.Xml, ParameterDirection.Input);
            parameters.Add("@CreatedBy", user.CreatedBy, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Inventory_AddUser";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool UpdateUser(UpdateUser user)
        {
            var parameters = new DynamicParameters();

            StringBuilder accessListXml = new StringBuilder();

            foreach (var item in user.AccessList)
            {
                accessListXml.Append("<Access>");
                accessListXml.AppendFormat("<MenuID>{0}</MenuID>", item);
                accessListXml.Append("</Access>");
            }

            parameters.Add("@UserId", user.UserID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserName", user.UserName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Phone", user.Phone, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", user.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@AccessList", accessListXml.ToString(), DbType.Xml, ParameterDirection.Input);
            parameters.Add("@Password", user.Password, DbType.String, ParameterDirection.Input);
            parameters.Add("@ModifiedBy", user.ModifiedBy, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.InventoryUser_UpdateUser";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }


        public bool DeleteUser(int userID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Inventory_DeleteUser";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;

            }
        }
        //---------------------------------------Group Details 08-02-2021---------------------------//
        public bool AddGroup(AddGroup user)
        {
            var parameters = new DynamicParameters();

            StringBuilder accessListXml = new StringBuilder();

            foreach (var item in user.AccessList)
            {
                accessListXml.Append("<Access>");
                accessListXml.AppendFormat("<ItemMasterID>{0}</ItemMasterID>", item);
                accessListXml.Append("</Access>");
            }

            parameters.Add("@UserID", user.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@ItemMasterID", user.ItemMasterID, DbType.String, ParameterDirection.Input);
            parameters.Add("@Del", user.Del, DbType.String, ParameterDirection.Input);
            parameters.Add("@AccessList", accessListXml.ToString(), DbType.Xml, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Inventory_AddGroup";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool UpdateGroup(UpdateGroup userID)
        {
            var parameters = new DynamicParameters();

            StringBuilder accessListXml = new StringBuilder();

            foreach (var item in userID.AccessList)
            {
                accessListXml.Append("<Access>");
                accessListXml.AppendFormat("<MenuID>{0}</MenuID>", item);
                accessListXml.Append("</Access>");
            }

            parameters.Add("@GID", userID.GID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@UserID", userID.UserID, DbType.String, ParameterDirection.Input);
            parameters.Add("@ItemMasterID", userID.ItemMasterID, DbType.String, ParameterDirection.Input);
            parameters.Add("@Del", userID.Del, DbType.String, ParameterDirection.Input);
           
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.InventoryUser_UpdateGroup";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }


        public bool DeleteGroup(int GID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@GID", GID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Inventory_DeleteGroup";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;

            }
        }

        public AddGroup GetGroupByID(int GID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@GID", GID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.InventoryUser_GetGroupByID";

                var result = connection.QueryMultiple(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                var user = result.Read<AddGroup>().First();

                List<int> accessList = result.Read<int>().ToList();

                user.AccessList = accessList.ToArray();

                return user;
            }
        }

        public List<AddGroup> GetGroupDetails(out int totalCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            var parameters = new DynamicParameters();

            string whereClause = string.Empty;

            string countQuery = string.Format(@"SELECT COUNT(*) FROM GroupControll WHERE Del = 'N'", whereClause);

            string query = string.Format(@"SELECT G.GID,G.UserID,UserName,G.ItemMasterID,ItemName,Del
                                           FROM GroupControll G JOIN AppUsers AU ON G.UserID = AU.UserID 
                                           JOIN ItemMasters I ON G.ItemMasterID = I.ItemMasterID
                                           WHERE G.Del = 'N'{2}
                            ORDER BY UserName DESC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY;", jtStartIndex, jtPageSize, whereClause);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                
                var reader = connection.QueryMultiple(countQuery + "\n" + query, parameters);

                totalCount = reader.Read<int>().Single();

                var result = reader.Read<AddGroup>();

                return result.ToList();
            }
        }
        //-----------------------------------------------------------------------------------------//
        public AddUser GetUserByID(int userID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserID", userID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.InventoryUser_GetUserByID";
                
                var result = connection.QueryMultiple(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                var user = result.Read<AddUser>().First();

                List<int> accessList = result.Read<int>().ToList();

                user.AccessList = accessList.ToArray();

                return user;
            }
        }

        public bool CheckUserExistance(string userName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserName", userName, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Inventory_CheckExistance";

                var result = connection.Query(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                return result.ToList().Count > 0;
            }
        }

        public WebSetting GetWebSetting()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string query = "SELECT CompanyName,CompanyLogo " +
                                     "FROM Inventory_WebSetting ";

                List<WebSetting> setting = connection.Query<WebSetting>(query).ToList();

                return setting.FirstOrDefault();
            }

        }

        public bool UpdateSettings(string fileName, string filePath, string companyName)
        {
            var parameters = new DynamicParameters();
            
            parameters.Add("@CompanyLogo", fileName, DbType.String, ParameterDirection.Input);
            parameters.Add("@CompanyName", companyName, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Inventory_UpdateSettings";
                IDbTransaction transaction = connection.BeginTransaction();
                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;

            }
        }
    }
}
