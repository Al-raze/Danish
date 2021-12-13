using Infrastructure.ConfigurationProvider;
using Infrastructure.Core.DataAccess;
using Infrastrucutre.Core.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DapperExtensions;

namespace Infrastrucutre.Core.DataAccess
{
    public class SupplierRepository : ISupplierRepository
    {

        public List<Supplier> GetSuppliers()
        {
            var parameters = new DynamicParameters();

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                const string storedProcedure = "dbo.Supplier_GetAllSuppliers";
                IEnumerable<Supplier> items = connection.Query<Supplier>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return items.ToList();
            }


        }

        public Supplier GetSupplierByID(int supplierID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                Supplier item = connection.Get<Supplier>(supplierID);

                return item;
            }
        }

        public bool UpdateSupplier(Supplier s)
        {
            bool supplierUpdated = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                supplierUpdated = connection.Update<Supplier>(s);

                return supplierUpdated;
            }
        }


        public bool AddSupplier(Supplier s)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@SupplierName", s.SupplierName, DbType.String, ParameterDirection.Input);
            parameters.Add("@Address", s.Address, DbType.String, ParameterDirection.Input);
            parameters.Add("@Email", s.Email, DbType.String, ParameterDirection.Input);
            parameters.Add("@PhoneNumber", s.PhoneNumber, DbType.String, ParameterDirection.Input);
            parameters.Add("@SalesRepName", s.SalesRepName, DbType.String, ParameterDirection.Input);
            parameters.Add("@WebAddress", s.WebAddress, DbType.String, ParameterDirection.Input);
            parameters.Add("@BankDetails", s.BankDetails, DbType.String, ParameterDirection.Input);

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                const string storedProcedure = "dbo.Supplier_AddSupplier";
                IDbTransaction transaction = connection.BeginTransaction();
                int rowsaffected = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                transaction.Commit();
                return rowsaffected > 0;
            }
        }


    }
}
