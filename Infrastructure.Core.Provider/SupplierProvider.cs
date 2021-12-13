
using Infrastrucutre.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Provider
{
    public class SupplierProvider : ISupplierProvider
    {
        ISupplierRepository _supplierRepository;

        public SupplierProvider(ISupplierRepository _supplierRepository)
        {
            this._supplierRepository = _supplierRepository;
        }

        public List<Supplier> GetSuppliers()
        {
            return _supplierRepository.GetSuppliers();
        }

        public bool AddSupplier(Supplier s)
        {
            return _supplierRepository.AddSupplier(s);
        }


        public Supplier GetSupplierByID(int supplierID)
        {
            return _supplierRepository.GetSupplierByID(supplierID);
        }


        public bool UpdateSupplier(Supplier s)
        {
            return _supplierRepository.UpdateSupplier(s);
        }
    }
}
