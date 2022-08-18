using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OWLOSAirQaulityUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWLOSAirQulityAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public DataController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("AirQuality")]
        public async Task<IActionResult> AirQuality()
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
