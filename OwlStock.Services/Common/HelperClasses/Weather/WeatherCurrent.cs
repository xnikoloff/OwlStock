using Newtonsoft.Json;

namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class WeatherCurrent
    {
        [JsonProperty("location")]
        public Location? Location { get; set; }

        [JsonProperty("current")]
        public Current? Current { get; set; }
    }
}
