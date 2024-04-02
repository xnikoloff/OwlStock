using Newtonsoft.Json;

namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class Forecast
    {
        [JsonProperty("forecastday")]
        public IEnumerable<ForecastDay>? ForecastDays { get; set; }
    }
}
