using OWLOSAirQaulityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWLOSAirQualityAPI.Businesses.Interfaces
{
    interface IAirQualityClientBusiness
    {
        public void AddData(string controllerToken, List<AirQualityDataItemDTO> data);
    }
}
