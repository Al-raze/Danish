using Infrastructure.Core.DataAccess;
using Infrastructure.Core.Models;
using Infrastructure.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AkraTechFramework.Controllers
{
    [Authorize]
    public class SampleController : BaseController
    {
        //
        // GET: /Sample/

        private IMembershipProvider _memberShipProvider;

        public SampleController(IMembershipProvider membershipProvider)
        {
            this._memberShipProvider = membershipProvider;
        }

        public SampleController() : this(new CustomMembershipProvider(new UserRepository()))
        {

        }

        public ActionResult Index()
        {
            return View();
        }
       
             
        public ActionResult UserDetails()
        {
            List<UserInformation> userDetails = _memberShipProvider.GetUserDetails();

            var flexGridView = new FlexGridView();
            var flexGridRows = new List<FlexiGridRow>();
            foreach (var item in userDetails)
            {
                var gridRow = new FlexiGridRow();
                gridRow.cell = new List<string>();
                gridRow.cell.Add(item.UserID.ToString());
                gridRow.cell.Add(item.UserName);
                gridRow.cell.Add(item.Phone);
                gridRow.cell.Add(item.Location);
                flexGridRows.Add(gridRow);

            }
            flexGridView.total = userDetails.Count;
            flexGridView.rows = flexGridRows;

            return Json(flexGridView, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUser(UserInformation user)
        {
            bool success = _memberShipProvider.AddUser(user);
            if (success)
                Success("User Added Successfully");
            else
                Error("Error in adding user");

            ModelState.Clear();

            return View("Index");
        }



        public ActionResult EditUser(UserInformation user)
        {
            bool success = _memberShipProvider.EditUser(user);
            if (success)
                Success("User Updated Successfully");
            else
                Error("Error in updating user");

            ModelState.Clear();

            return View("Index");
        }


        public ActionResult Invoice()
        {
            return View();
        }

        public ActionResult Index1()
        {
            return View("Index");
        }
    }
}
