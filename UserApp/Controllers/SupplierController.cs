using AkraTechFramework.Controllers;
using Infrastructure.Core.Provider;
using Infrastrucutre.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManager.Controllers
{
    [Authorize]
    public class SupplierController : BaseController
    {
        
        ISupplierProvider _supplierProvider;
        public SupplierController(ISupplierProvider supplierProvider)
        {
            this._supplierProvider = supplierProvider;
        }

        public SupplierController()
            : this(new SupplierProvider(new SupplierRepository()))
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetSuppliers()
        {
            var items = _supplierProvider.GetSuppliers();
            return Json(new { Result = "OK", Records = items });
        }



        public ActionResult AddSupplier()
        {
            ViewBag.Mode = "Create";
            return View();
        }

        public ActionResult GetSupplierByID(int id)
        {
            Supplier item = _supplierProvider.GetSupplierByID(id);                        
            return View("AddSupplier", item);
        }

        [HttpPost]
        public ActionResult UpdateSupplier(Supplier item)
        {
            bool IsUpdated = false;
            if (ModelState.IsValid)
            {
                IsUpdated = _supplierProvider.UpdateSupplier(item);
            }

            if (IsUpdated)
            {
                Success("Supplier Created Successfully.");
                return RedirectToAction("Index");
            }
            else
            {
                Attention("Please verify the fields you have entered");
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult AddSupplier(Supplier item, string command)
        {
            bool IsUpdated = false;
            if (ModelState.IsValid)
            {
                if (command == "Save")
                {
                    IsUpdated = _supplierProvider.AddSupplier(item);
                }
                else if (command == "Update")
                {
                    IsUpdated = _supplierProvider.UpdateSupplier(item);
                }
            }

            if (IsUpdated)
            {
                if (command == "Save")
                {
                    Success("Supplier Created Successfully.");
                }
                else
	            {
                    Success("Supplier Updated Successfully.");
                }

                return RedirectToAction("Index");
            }
            else
            {
                Attention("Please verify the fields you have entered");
                return View(item);
            }

        }
    }
}
