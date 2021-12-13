using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.ConfigurationProvider;
using Infrastructure.Core.DataAccess;
using Infrastructure.Core.Models;

namespace Infrastructure.Core.DataAccess
{
    public class TenderRepository : ITenderRepository
    {

        public bool AddTender(AddTender tender)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyID", tender.CompanyID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@TenderName", tender.TenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@TenderNo", tender.TenderNo, DbType.String, ParameterDirection.Input);
            parameters.Add("@PurchaseValue", tender.PurchaseValue, DbType.Double, ParameterDirection.Input);
            parameters.Add("@PurchaseDate", tender.PurchaseDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@ClosingDate", tender.ClosingDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@BankID", tender.BankID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@BankTypeID", tender.BankTypeID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@StatusID", 1, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy",tender.CreatedBy, DbType.Int32, ParameterDirection.Input);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Tender_AddTender";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool UpdateTender(ModifyTender tender)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@TenderName", tender.TenderName, DbType.String, ParameterDirection.Input);
            parameters.Add("@StatusID", tender.StatusID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@PurchaseValue", tender.PurchaseValue, DbType.Currency, ParameterDirection.Input);
            parameters.Add("@DaysRequired", tender.DaysRequired, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ProjectValue", tender.ProjectValue, DbType.Currency, ParameterDirection.Input);
            parameters.Add("@GuaranteeValue", tender.GuaranteeValue, DbType.Currency, ParameterDirection.Input);
            parameters.Add("@ProportionValue", tender.ProportionValue, DbType.Currency, ParameterDirection.Input);
            parameters.Add("@GuaranteeStartDate", tender.GuaranteeStartDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@GuaranteeEndDate", tender.GuaranteeEndDate, DbType.String, ParameterDirection.Input);
            parameters.Add("@ModifiedBy",tender.ModifiedBy, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@TenderID", tender.TenderID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Tender_UpdateTender";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();
                return rowsaffected > 0;
            }
        }

        public List<Tender> GetTenderDetails(int StatusID)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@StatusID", StatusID, DbType.Int32, ParameterDirection.Input);
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Tender_GetDetails";

                IEnumerable<Tender> gettenderDetails = connection.Query<Tender>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return gettenderDetails.ToList();
            }
        }

        public List<Tender> GetTenderStatus(Tender tender)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@StatusID", tender.StatusID, DbType.Int32, ParameterDirection.Input);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Tender_GetTenderStatus";
                IEnumerable<Tender> gettenderStatus = connection.Query<Tender>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return gettenderStatus.ToList();
            }
        }

        public bool DeleteTender(int tenderID)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@TenderID", tenderID, DbType.Int32, ParameterDirection.Input);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Tender_DeleteTender";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();
                return rowsaffected > 0;
            }
        }

        public ModifyTender GetTenderByID(int tenderID)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@TenderID", tenderID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Tender_GetTenderByID";

                IEnumerable<ModifyTender> tender = connection.Query<ModifyTender>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                return tender.FirstOrDefault();
            }
        }


        public List<Company> GetCompanyDetails()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Company_GetCompany";

                IEnumerable<Company> company = connection.Query<Company>(storedProcedure, commandType: CommandType.StoredProcedure);

                return company.ToList();
            }
        }

        public List<Tender> GetBankNameDetails()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Bank_GetBankName";

                IEnumerable<Tender> bank = connection.Query<Tender>(storedProcedure, commandType: CommandType.StoredProcedure);

                return bank.ToList();
            }
        }


        public List<Rival> GetRivalDetails(int tenderID)
        {
            var parameters = new DynamicParameters();            
            parameters.Add("@TenderID", tenderID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Rival_GetRivalDetails";
                IEnumerable<Rival> getRival = connection.Query<Rival>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getRival.ToList();
            }
        }


        public Rival GetRivalByID(int rivalID)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@RivalID", rivalID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Rival_GetRivalByID";

                IEnumerable<Rival> rival = connection.Query<Rival>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                return rival.FirstOrDefault();
            }
        }

        public bool CreateRival(Rival rival)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RivalMasterID", rival.RivalMasterID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@TenderID", rival.TenderID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ProjectValue", rival.ProjectValue, DbType.Double, ParameterDirection.Input);
            parameters.Add("@IncrementValue", rival.IncrementValue, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Discount", rival.Discount, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Note", rival.Note, DbType.String, ParameterDirection.Input);
            parameters.Add("@CreatedBy", 1, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Rival_CreateRival";

                IDbTransaction transaction = connection.BeginTransaction();
                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();
                return rowsaffected > 0;
            }
        }

        public bool UpdateRivalDetails(Rival rival)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RivalID", rival.RivalID, DbType.Int32, ParameterDirection.Input);            
            parameters.Add("@ProjectValue", rival.ProjectValue, DbType.Double, ParameterDirection.Input);
            parameters.Add("@IncrementValue", rival.IncrementValue, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Discount", rival.Discount, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Note", rival.Note, DbType.String, ParameterDirection.Input);;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Rival_UpdateRivalDetails";

                IDbTransaction transaction = connection.BeginTransaction();
                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();
                return rowsaffected > 0;
            }
        }

        public List<Rival> GetTenderName()
        {
            var parameters = new DynamicParameters();
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Rival_GetTenderName";

                IEnumerable<Rival> getRival = connection.Query<Rival>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getRival.ToList();
            }
        }

        public List<Rival> GetRivalName()
        {
            var parameters = new DynamicParameters();
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Rival_GetRivalName";

                IEnumerable<Rival> getRival = connection.Query<Rival>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return getRival.ToList();
            }
        }

        public List<Tender> GetTenderStatusName()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.GetTenderStatusName";

                IEnumerable<Tender> company = connection.Query<Tender>(storedProcedure, commandType: CommandType.StoredProcedure);

                return company.ToList();
            }
        }

        public List<Tender> GetUploadDocumentFile(int tenderID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@TenderID", tenderID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.GetUploadDocumentFile";

                IEnumerable<Tender> gettenderDoc = connection.Query<Tender>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return gettenderDoc.ToList();
            }
        }

        public bool Uploadfile(string fileName, string filePath,int tenderID)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FileName", fileName, DbType.String, ParameterDirection.Input);
            parameters.Add("@path", filePath, DbType.String, ParameterDirection.Input);
            parameters.Add("@TenderID", tenderID, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "Tender_UploadDocumentFile";

                IDbTransaction transaction = connection.BeginTransaction();
                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();
                return rowsaffected > 0;
            }
        }

        public List<Tender> GetTenderNotifications()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Tender_GetTenderNotifications";

                IEnumerable<Tender> tender = connection.Query<Tender>(storedProcedure, commandType: CommandType.StoredProcedure);

                return tender.ToList();
            }
        }

        public bool DeleteUploadDoc(int FileID)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@FileID", FileID, DbType.Int32, ParameterDirection.Input);


            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Tender_DeleteTenderDoc";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();
                return rowsaffected > 0;
            }
        }

        public bool DeleteRivals(int id)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@RivalID", id, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Tender_DeleteRivals";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool AddColor(int[] rivalID, string colorCode)
        {
            var parameters = new DynamicParameters();

            string itemsXml = string.Empty;
            itemsXml += "<AddColor>";

            foreach (var item in rivalID)
            {
                itemsXml += string.Format("<Access  RivalID=\"{0}\"/>", item);

            }

            itemsXml += "</AddColor>";

            parameters.Add("@ColorCode", colorCode, DbType.String, ParameterDirection.Input);
            parameters.Add("@RivalID", itemsXml, DbType.Xml, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Tender_AddColors";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }
    }
}
