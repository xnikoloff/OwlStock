using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class Location
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("region")]
        public string? Region { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("lat")]
        public string? Latitude { get; set; }

        [JsonProperty("lon")]
        public string? Longitude { get; set; }
    }
}
