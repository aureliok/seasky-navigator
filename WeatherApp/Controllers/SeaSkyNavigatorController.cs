using Microsoft.AspNetCore.Mvc;
using SeaSkyNavigator.Classes;
using SeaSkyNavigator.Records;
using SeaSkyNavigator.Helpers;
using System;
using System.Text.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cors;

namespace SeaSkyNavigator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class SeaSkyNavigatorController : ControllerBase
    {
        private CoordinatesGetter _coordinatesGetter;
        private WeatherService _weatherService;
        private LocationData locationData;
        private Hourly hourlyForecast = new Hourly();
        private readonly LLMChat chat = new LLMChat();

        public SeaSkyNavigatorController(CoordinatesGetter coordinatesGetter)
        {
            _coordinatesGetter = coordinatesGetter ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Get a weather report from the SeaSky Navigator
        /// </summary>
        /// <param name="address">The address/location for which to retrieve coordinates</param>
        /// <param name="startDate">Start date for the forecast (yyyy-MM-dd)</param>
        /// <param name="daysAheadForecast">Number of days (integer) to include in the weather forecast</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /SeaSkyNavigator/GiveMeTheForecastMate
        ///     {
        ///        "address": "São Paulo",
        ///        "startDate": "2024-03-02",
        ///        "daysAheadForecast": 2
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>

        [HttpGet]
        [Route("GiveMeTheForecastMate")]
        [SwaggerOperation(Summary = "Get a weather report from our SeaSky Navigator")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(400, "Bad Request")]
        public async Task<IActionResult> GetCoordinates(
            [FromQuery]
            [Required]
            string address,
            [FromQuery]
            [Required]
            string startDate,
            [FromQuery]
            [Required]
            string daysAheadForecast)
        {
            Task<string> coordinatesResponse = _coordinatesGetter.GetCoordinates(address);
            Location? location = JsonSerializer.Deserialize<Location>(coordinatesResponse.Result);

            locationData = location.data[0];

            _weatherService = new WeatherService(startDate, daysAheadForecast);

            Task<string> weatherResponse = _weatherService.GetWeatherData(locationData.latitude, locationData.longitude);
            WeatherForecast? weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(weatherResponse.Result);

            ForecastHelper.KeepOnlyActualForecast(hourlyForecast, weatherForecast);
            string forecastDataString = ForecastHelper.CreateForecastDataString(hourlyForecast);

            string weatherReport = await chat.GiveMeTheWeatherReportMate(forecastDataString, address);

            return Ok(weatherReport);
        }
    }
}
