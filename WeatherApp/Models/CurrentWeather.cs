using Newtonsoft.Json;

namespace WeatherApp.Models
{
    public class CurrentWeather
    {

        public double Temperature { get; set; }
        public string Description { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int Cloud { get; set; }
        public string Icon { get; set; }
    }
}

