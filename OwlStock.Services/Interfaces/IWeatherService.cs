using OwlStock.Services.Common.HelperClasses.Weather;

namespace OwlStock.Services.Interfaces
{
    public interface IWeatherService
    {
        public Task<WeatherForecast> GetForecast(string settlementId);
        public Task<WeatherCurrent> GetCurrentWeather(string settlement);
        public Task<IEnumerable<SettlementInfo>> Autocomplete(string name);
    }
}
