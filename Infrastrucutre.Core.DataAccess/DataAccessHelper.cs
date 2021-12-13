using DapperExtensions.Mapper;
using Infrastructure.ConfigurationProvider;
using Infrastrucutre.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;


namespace Infrastructure.Core.DataAccess
{
    public class DataAccessHelper
    {
        public static IDbConnection OpenSqlConnection(IConnectionStringProvider connectionStringProvider)
        {
            IDbConnection connection = new SqlConnection(connectionStringProvider.GetConnectionString());
            
            connection.Open();
            return connection;
        }

        static DataAccessHelper()
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(CustomPluralizedMapper<>);
        }

    }

    public class CustomPluralizedMapper<T> : PluralizedAutoClassMapper<T> where T : class
    {
        public override void Table(string tableName)
        {
            //switch (tableName.ToUpper())
            //{
            //    case "ITEMCOLOR" :
            //        this.TableName = "ItemColors";
            //        break;               
            //    default:
            //        break;
            //}
          
            base.Table(tableName);
        }
    }
}
