using Newtonsoft.Json;

namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class Day
    {
        [JsonProperty("maxtemp_c")]
        public string? MaxTemperature { get; set; }

        [JsonProperty("mintemp_c")]
        public string? MinTemperature { get; set; }

        [JsonProperty("avgtemp_c")]
        public string? AverageTemperature { get; set; }

        [JsonProperty("totalprecip_mm")]
        public string? TotalPrecipitation { get; set; }

        [JsonProperty("condition")]
        public Condition? Condition { get; set; }
    }
}
