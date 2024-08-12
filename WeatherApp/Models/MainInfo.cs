using Newtonsoft.Json;

namespace WeatherApp.Models
{
    public class MainInfo
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }
        public int Humidity { get; set; }
    }
}
