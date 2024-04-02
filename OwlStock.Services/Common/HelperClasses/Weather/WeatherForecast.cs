namespace OwlStock.Services.Common.HelperClasses.Weather
{
    public class WeatherForecast
    {
        public Location? Location { get; set; }
        public Current? Current { get; set; }
        public Forecast? Forecast { get; set; }
    }
}
