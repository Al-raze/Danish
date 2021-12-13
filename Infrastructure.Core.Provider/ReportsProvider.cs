using Infrastrucutre.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Provider
{
    public class ReportsProvider : IReportsProvider
    {
        IReportsRepository _reportsRepository;

        public ReportsProvider(IReportsRepository reportsRepository)
       {
           _reportsRepository = reportsRepository;
       }

        public List<Report> GetReportList()
        {
            return _reportsRepository.GetReportList();                
        }

        public List<dynamic> GetReportById(string startDate, string endDate, int reportId, string filter)
        {
            return _reportsRepository.GetReportById(startDate, endDate, reportId, filter);  
        }
    }
}
