using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucutre.Core.DataAccess
{
    public interface IReportsRepository
    {
        List<Report> GetReportList();

        List<dynamic> GetReportById(string startDate, string endDate, int reportId, string filter);
    }
}
