using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Provider
{
    public interface ICarrierProvider
    {
        bool AddCarrier(Infrastrucutre.Core.Models.PostalCarrier carrier);
        Infrastrucutre.Core.Models.PostalCarrier GetCarrierByID(int carrierID);
        bool DeleteCarrierByID(int PostalCarrierID);//Delete Record Developed By Danish 28-01-2021
        System.Collections.Generic.List<Infrastrucutre.Core.Models.PostalCarrier> GetCarriers();
        bool UpdateCarrier(Infrastrucutre.Core.Models.PostalCarrier carrier);
        bool UpdateCarrierImage(Infrastrucutre.Core.Models.PostalCarrier carrier);
    }
}
