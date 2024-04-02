using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class Current
    {
        [JsonProperty("temp_c")]
        public string? Temperature { get; set; }

        [JsonProperty("condition")]
        public Condition? Condition { get; set; }

        [JsonProperty("precip_mm")]
        public string? Precipitation { get; set; }
    }
}
