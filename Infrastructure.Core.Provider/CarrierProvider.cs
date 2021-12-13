using Infrastrucutre.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Provider
{
   public  class CarrierProvider : ICarrierProvider
    {
        ICarrierRepository _carrierRepository;

        public CarrierProvider(ICarrierRepository carrierRepository)
        {
            this._carrierRepository = carrierRepository;
        }


        public bool AddCarrier(Infrastrucutre.Core.Models.PostalCarrier carrier)
        {
            return _carrierRepository.AddCarrier(carrier);
        }

        public Infrastrucutre.Core.Models.PostalCarrier GetCarrierByID(int carrierID)
        {
            return _carrierRepository.GetCarrierByID(carrierID);
        }
        //------------------------DELETE CARRIER BY DANISH  28-01-2021------------------------------------//
        public bool DeleteCarrierByID(int PostalCarrierID)
        {
            return _carrierRepository.DeleteCarrierByID(PostalCarrierID);
        }
        //------------------------------------------------------------------------------------------------------//
        public List<Infrastrucutre.Core.Models.PostalCarrier> GetCarriers()
        {
            return _carrierRepository.GetCarriers();
        }

        public bool UpdateCarrier(Infrastrucutre.Core.Models.PostalCarrier carrier)
        {
            return _carrierRepository.UpdateCarrier(carrier);
        }


        public bool UpdateCarrierImage(Infrastrucutre.Core.Models.PostalCarrier carrier)
        {
            return _carrierRepository.UpdateCarrierImage(carrier);
        }
    }
}
