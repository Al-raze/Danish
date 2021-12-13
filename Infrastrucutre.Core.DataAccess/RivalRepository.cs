using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.ConfigurationProvider;
using Infrastructure.Core.Models;
using Infrastructure.Core.DataAccess;

namespace Infrastructure.Core.DataAccess
{
    public class RivalRepository
    {
        public bool CreateRival(Rival rival)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RivalMasterID", rival.RivalMasterID ,DbType.Int32, ParameterDirection.Input);
            parameters.Add("@TenderID", rival.TenderID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@ProjectValue", rival.ProjectValue, DbType.Double, ParameterDirection.Input);
            parameters.Add("@IncrementValue", rival.IncrementValue, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Discount", rival.Discount, DbType.Double, ParameterDirection.Input);
            parameters.Add("@Note", rival.Note, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsActive", rival.IsActive, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", rival.CreatedBy, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Rival_CreateRival";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }

        public bool AddRivalDetails(Rival rival)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@RivalName", rival.RivalName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address1", rival.Address1, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address2", rival.Address2, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address3", rival.Address3, DbType.String, ParameterDirection.Input);
            parameters.Add("@IsActive", rival.IsActive, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@CreatedBy", rival.CreatedBy, DbType.Int32, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Rival_AddRivalDetails";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }
    }
}
