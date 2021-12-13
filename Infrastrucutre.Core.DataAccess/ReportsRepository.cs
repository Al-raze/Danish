using Dapper;
using Infrastructure.ConfigurationProvider;
using Infrastructure.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;

namespace Infrastrucutre.Core.DataAccess
{
    public class ReportsRepository : IReportsRepository
    {
        public List<Report> GetReportList()
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {
                var predicate = Predicates.Field<Report>(f => f.IsActive, Operator.Eq,true);

                var list = connection.GetList<Report>(predicate);

                return list.ToList();
            }
        }

        public List<dynamic> GetReportById(string startDate, string endDate, int reportId, string filter)
        {
            using (IDbConnection connection = DataAccessHelper.OpenSqlConnection(ConnectionStringManager.SqlConnectionStringInstance))
            {

                var predicate = Predicates.Field<Report>(f => f.ReportID, Operator.Eq, reportId);

                var report = connection.Get<Report>(reportId);

                filter = string.IsNullOrWhiteSpace(filter) ? "" : " AND " + filter.Trim();

                var list = connection.Query<dynamic>(report.ReportQuery + filter, new { startDate, endDate });

                return list.ToList();
            }


        }

    }

}
