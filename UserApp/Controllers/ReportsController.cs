using Infrastructure.Constants;
using Infrastructure.Core.Models;
using Infrastructure.Core.Provider;
using Infrastrucutre.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace AkraTechFramework.Controllers
{
    [Authorize]
    public class ReportsController : BaseController
    {
        //
        // GET: /Reports/

        IReportsProvider _reportProvider;        

        public ReportsController(IReportsProvider reportProvider)
        {
            this._reportProvider = reportProvider;

        }


        public ReportsController()
            : this(new ReportsProvider(new ReportsRepository()))
        {

        }

        public ActionResult Index()
        {
            var items = _reportProvider.GetReportList();

            ViewBag.ReportID = new SelectList(items, "ReportID", "ReportName");

            return View();
        }

        public ActionResult AsinReport()
        {
            return View();
        }


        public JsonResult GetReport(string startDate = null, string endDate = null,string reportId=null,string filter="")
        {
            try
            {
                var dateTimeFrom = DateTime.Parse(startDate);
                var dateTimeTo = DateTime.Parse(endDate);

                var value = dateTimeTo.Subtract(dateTimeFrom);

                if (value.Days < 0)
                {
                    throw new Exception("Please select a valid date range,To Date cannot be lesser than the from date");

                }
                else if (value.Days > 180)
                {
                    throw new Exception("You can only select from and to date in the range of 180 days");
                }
                else
                {
                    var data = _reportProvider.GetReportById(startDate, endDate, int.Parse(reportId), filter);

                    return Json(new { Result = data });
                }

                return Json(new { Result = "ERROR" });
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR" , Message = ex.Message });
            }
            
        }

    }
}
