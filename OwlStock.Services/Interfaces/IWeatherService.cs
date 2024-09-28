using OwlStock.Services.Common.HelperClasses.Weather;

namespace OwlStock.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherForecast> GetForecast(string settlementId);
        Task<WeatherForecast> GetForecastForPlace(Guid placeId);
        Task<WeatherCurrent> GetCurrentWeather(string settlement);
        Task<IEnumerable<SettlementInfo>> Autocomplete(string name);
    }
}
