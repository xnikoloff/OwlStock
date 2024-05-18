using Microsoft.Extensions.Configuration;
using OwlStock.Services.Common.HelperClasses.Weather;
using OwlStock.Services.Interfaces;
using Newtonsoft.Json;
using OwlStock.Infrastructure;
using OwlStock.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace OwlStock.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly int _days = 14;
        private readonly string _language = "bg";
        private readonly string _apiKey;

        private readonly string _host;
        private readonly IConfiguration _configuration;
        private readonly ISettlementService _settlementService;

        public WeatherService(IConfiguration configuration, ISettlementService settlementService)
        {
            _configuration = configuration;
            _host = _configuration.GetSection("Weather").GetSection("Host").Value!;
            _apiKey = configuration.GetSection("Weather").GetSection("Key").Value!;
            _settlementService = settlementService;
        }
        
        public async Task<WeatherCurrent> GetCurrentWeather(string settlement)
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri(_host);
            
            string url = Path.Combine(_host, _configuration.GetSection("Weather").GetSection("Current").Value! + $"?q={settlement}&lang={_language}&key={_apiKey}");
            HttpResponseMessage response = await client.GetAsync(url);

            string json = await response.Content.ReadAsStringAsync();
            WeatherCurrent? forecast = JsonConvert.DeserializeObject<WeatherCurrent>(json);

            return forecast ?? throw new NullReferenceException($"{nameof(forecast)} is null");
        }

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
            HttpResponseMessage response = await client.GetAsync(url);

            string json = await response.Content.ReadAsStringAsync();
            WeatherForecast? forecast = JsonConvert.DeserializeObject<WeatherForecast>(json);

            return forecast ?? throw new NullReferenceException($"{nameof(forecast)} is null");
        }

        public async Task<IEnumerable<SettlementInfo>> Autocomplete(string name)
        {
                using HttpClient client = new();
            client.BaseAddress = new Uri(_host);
            string url = Path.Combine(_host, _configuration.GetSection("Weather").GetSection("Autocomplete").Value! + "?access_key=" + _apiKey + "&query=" + name);
            HttpResponseMessage response = await client.GetAsync(url);

            string json = await response.Content.ReadAsStringAsync();
            IEnumerable<SettlementInfo>? forecast = JsonConvert.DeserializeObject<IEnumerable<SettlementInfo>>(json);

            return forecast ?? throw new NullReferenceException($"{nameof(forecast)} is null");
        }
    }
}
