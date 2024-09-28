using Microsoft.AspNetCore.Mvc;
using OwlStock.Domain.Entities;
using OwlStock.Services.Common.HelperClasses.Weather;
using OwlStock.Services.Interfaces;

namespace OwlStock.Web.Controllers.API
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ISettlementService _settlementService;

        public WeatherController(IWeatherService weatherService, ISettlementService settlementService)
        {
            _weatherService = weatherService;
            _settlementService = settlementService;
        }

        [HttpGet]
        [Route("forecast")]
        public async Task<WeatherForecast> GetForecast(string settlementId)
        {
            WeatherForecast foreacast = await _weatherService.GetForecast(settlementId);
            return foreacast;
        }

        [HttpGet]
        [Route("forecastForPlace")]
        public async Task<WeatherForecast> GetForecastForPlace(Guid settlementId)
        {
            WeatherForecast foreacast = await _weatherService.GetForecastForPlace(settlementId);
            return foreacast;
        }

        [HttpGet]
        [Route("currentWeather")]
        public async Task<WeatherCurrent> GetCurrentWeather(string settlement)
        {
            WeatherCurrent weatherInfo = await _weatherService.GetCurrentWeather(settlement);
            return weatherInfo;
        }

        [HttpGet]
        [Route("autocomplete")]
        public async Task<IEnumerable<City>> GetAutocomplete(string query)
        {
            var result = await _settlementService.Autocomplete(query);
            return result;
        }
    }
}
