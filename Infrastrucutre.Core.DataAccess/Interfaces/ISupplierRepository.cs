using System;
using Infrastrucutre.Core.Models;
using System.Collections.Generic;

namespace Infrastrucutre.Core.DataAccess
{
    public interface ISupplierRepository
    {
        bool AddSupplier(Supplier s);
        List<Supplier> GetSuppliers();
        Supplier GetSupplierByID(int supplierID);
        bool UpdateSupplier(Supplier s);
    }
}
