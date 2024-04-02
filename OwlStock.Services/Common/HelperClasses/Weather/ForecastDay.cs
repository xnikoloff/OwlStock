using Newtonsoft.Json;

namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class ForecastDay
    {
        [JsonProperty("day")]
        public Day? Day { get; set; }
    }
}
