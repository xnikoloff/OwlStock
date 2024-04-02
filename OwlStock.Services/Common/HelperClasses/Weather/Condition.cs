using Newtonsoft.Json;

namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class Condition
    {
        [JsonProperty("text")]
        public string? Text { get; set; }

        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }
}
