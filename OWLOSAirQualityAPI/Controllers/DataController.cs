using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OWLOSAirQaulityDTO;
using OWLOSAirQaulityUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWLOSAirQulityAPI.Controllers
{
    /// <summary>
    /// sensor_name = "dht"
    /// sensor_type = temperature, pressure
    /// sensor_unitofmessure = Celcisus, kPa... 
    /// sensor_range 
    /// 
    /// value,unit
    /// controllerId,32.23;unitOfMessureId,date,sensorNameId
    /// 42.23;perc
    /// ----------------
    /// Barear=asda;ldk;alksd;aLK
    /// 
    /// 
    /// =13123123
    /// 1;123;2; 
    /// 2;123;2;
    /// 3;123;2;
    /// 1;213;3;
    /// =13123124
    /// 1;123;2; 
    /// 2;123;2;
    /// 3;123;2;
    /// 1;213;3;
    /// =13123125
    /// 1;123;2; 
    /// 2;123;2;
    /// 3;123;2;
    /// 1;213;3;
    /// 
    /// 
    /// 
    /// 
    /// Celsius=123[DwqeHT]
    /// Celsius=123[DHT]
    /// Celsius=123[DHT]
    /// 
    /// </summary>

    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public DataController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;

            
        }


        [HttpPost("Lookup")]
        public async Task<List<string>> Lookup()
        {
            List<string> result = new List<string>();
            result.Add("[unitofmesure]");
            result.Add("Celsius=1");
            result.Add("Forenheit=2");
            result.Add("Pressure=4");
            result.Add("[sensors]");
            result.Add("DHT=1");
            result.Add("BMP=2");
            result.Add("CSS=3");

            return result;
        }

        [HttpPost("AirQuality")]
        public async Task<IActionResult> AirQuality(List<AirQualityDataItemDTO> _data)
        {
            string data;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                data = await reader.ReadToEndAsync();
            }

            if (string.IsNullOrWhiteSpace(data))
            {
                CSV csv = new CSVConvert().Serialise(data);
                if (csv != null)
                {
                    //process data 
                }
            }

            return Ok(data.ToUpper());
        }
    }
}
