using Newtonsoft.Json;

namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class SettlementInfo
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("region")]
        public string? Region { get; set; }

        [JsonProperty("lon")]
        public string? Lon { get; set; }

        [JsonProperty("lat")]
        public string? Lat { get; set; }
    }
}
