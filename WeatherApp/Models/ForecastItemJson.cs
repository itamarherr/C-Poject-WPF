using Newtonsoft.Json;

namespace WeatherApp.Models
{
    public class ForecastItemJson
    {
        [JsonProperty("main")]
        public MainInfo Main { get; set; }

        [JsonProperty("weather")]
        public WeatherInfo[] Weather { get; set; }

        [JsonProperty("dt_txt")]
        public string DtTxt { get; set; }
    }
}
