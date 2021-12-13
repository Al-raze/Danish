using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.ConfigurationProvider;
using Infrastructure.Core.Models;

namespace Infrastructure.Core.DataAccess
{
    public class BankRepository
    {
        public bool AddBankName(Bank bank)
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

        public bool AddBankTypeName(Bank bank)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@TypeName", bank.TypeName, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Bank_AddBankType";

                IDbTransaction transaction = connection.BeginTransaction();

                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);

                transaction.Commit();

                return rowsaffected > 0;
            }
        }
    }
}
