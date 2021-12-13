using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Infrastructure.ConfigurationProvider;
using Infrastructure.Core.Models;
using Infrastrucutre.Core.Models;
using Infrastructure.Core.DataAccess;

namespace Infrastrucutre.Core.DataAccess
{
    public class CarrierRepository : Infrastrucutre.Core.DataAccess.ICarrierRepository
    {
        public List<PostalCarrier> GetCarriers()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                List<PostalCarrier> items = connection.GetList<PostalCarrier>().ToList();

                return items;
            }
        }


        public PostalCarrier GetCarrierByID(int carrierID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                PostalCarrier item = connection.Get<PostalCarrier>(carrierID);

                return item;
            }
        }

        public bool AddCarrier(PostalCarrier carrier)
        {
            int carrierID;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                carrierID = connection.Insert<PostalCarrier>(carrier);

                return carrierID > 0;
            }
        }

        public bool UpdateCarrier(PostalCarrier carrier)
        {
            bool updateCompleted = false;

            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                updateCompleted = connection.Update<PostalCarrier>(carrier);

                return updateCompleted;
            }
        }

        
        public bool UpdateCarrierImage(PostalCarrier carrier)
        {
            //int updateCompleted = 0;

            //using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            //{
            //    updateCompleted = connection.Execute(string.Format("Update PostalCarriers set  CarrierImage='{1}' where PostalCarrierID={0}",carrier.PostalCarrierID,carrier.CarrierImage),commandType:CommandType.Text);

            //    return updateCompleted>0;
            //}
            return true;
        }

        //---------------------------------------------DELETE POSTAL CARRIER BY DANISH 28-01-2021---------------------------------------//

        public bool DeleteCarrierByID(int PostalCarrierID)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                string query = string.Format("DELETE FROM [arsukeuro_mssql].[dbo].[PostalCarriers] WHERE PostalCarrierID={0}", PostalCarrierID);
                int rowsAffected = connection.Execute(query, commandType: CommandType.Text);
                return rowsAffected > 0;
            }

        }

        //-----------------------------------------------------------------------------------------------------------------------------//
    }
}
