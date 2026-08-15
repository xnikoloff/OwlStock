using Microsoft.Extensions.Configuration;
using OwlStock.Services.Common.HelperClasses.Weather;
using Newtonsoft.Json;
using OwlStock.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using OwlStock.Services.Interfaces;

namespace OwlStock.Services.Implementations
{
    public class WeatherService : IWeatherService
    {
        private readonly int _days = 14;
        private readonly string _language = "bg";
        private readonly string _apiKey;

        private readonly string _host;
        private readonly IConfiguration _configuration;
        private readonly ISettlementService _settlementService;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IConfiguration configuration, ISettlementService settlementService, ILogger<WeatherService> logger)
        {
            _configuration = configuration;
            _host = _configuration.GetSection("Weather").GetSection("Host").Value!;
            _apiKey = configuration.GetSection("Weather").GetSection("Key").Value!;
            _settlementService = settlementService;
            _logger = logger;
        }
        
        /// <summary>
        /// Gets the current weather information for a settlement
        /// </summary>
        /// <param name="settlement">Name of the settlement used as a keyword</param>
        /// <returns>WeatherCurrent entity with the required data</returns>
        public async Task<WeatherCurrent> GetCurrentWeather(string settlement)
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri(_host);
            
            string url = Path.Combine(_host, _configuration.GetSection("Weather").GetSection("Current").Value! + $"?q={settlement}&lang={_language}&key={_apiKey}");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                string json = await response.Content.ReadAsStringAsync();
                WeatherCurrent? forecast = JsonConvert.DeserializeObject<WeatherCurrent>(json);
                return forecast ?? new();
            }

            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        /// <summary>
        /// Gets information about the weather forecast for a settlement
        /// </summary>
        /// <param name="settlementId">Id of the settlement</param>
        /// <returns>WeatherForecast entity with the required data</returns>
        /// <exception cref="NullReferenceException">Thrown when the provided argument is null or empty</exception>
        public async Task<WeatherForecast> GetForecast(string settlementId)
        {
            if (settlementId.IsNullOrEmpty())
            {
                throw new NullReferenceException($"{nameof(settlementId)} is null or empty");
            }

            using HttpClient client = new();
            client.BaseAddress = new Uri(_host);

            City city = await _settlementService.GetCityById(int.Parse(settlementId));

            string url = Path.Combine(_host, _configuration.GetSection("Weather").GetSection("Forecast").Value! + $"?q={city.NameLatin}&days={_days}&lang={_language}&key={_apiKey}");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                string json = await response.Content.ReadAsStringAsync();
                WeatherForecast? forecast = JsonConvert.DeserializeObject<WeatherForecast>(json);

                return forecast ?? new();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }

        /// <summary>
        /// Gets the forecast for a place
        /// </summary>
        /// <param name="placeId">Id of the place</param>
        /// <returns>WeatherForecast entity with the required data</returns>
        /// <exception cref="NullReferenceException">Thrown when provided argument is null</exception>
        public async Task<WeatherForecast> GetForecastForPlace(Guid placeId)
        {
            if (placeId == Guid.Empty)
            {
                throw new NullReferenceException($"{nameof(placeId)} is null or empty");
            }

            using HttpClient client = new();
            client.BaseAddress = new Uri(_host);

            string placeSettlementName = await _settlementService.GetPopularPlaceSettlementName(placeId);

            string url = Path.Combine(_host, _configuration.GetSection("Weather").GetSection("Forecast").Value! + $"?q={placeSettlementName}&days={_days}&lang={_language}&key={_apiKey}");
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                string json = await response.Content.ReadAsStringAsync();
                WeatherForecast? forecast = JsonConvert.DeserializeObject<WeatherForecast>(json);

                return forecast ?? new();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred at {Time}", DateTime.UtcNow);
                return new();
            }
        }
    }
}
