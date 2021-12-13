using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Provider
{
    public interface ISupplierProvider
    {
        bool AddSupplier(Supplier s);
        List<Supplier> GetSuppliers();
        Supplier GetSupplierByID(int supplierID);
        bool UpdateSupplier(Supplier s);
    }
}
