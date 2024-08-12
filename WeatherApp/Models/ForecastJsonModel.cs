using Newtonsoft.Json;

namespace WeatherApp.Models
{
    public class ForecastJsonModel
    {
        [JsonProperty("list")]
        public List<ForecastItemJson> List { get; set; }
    }
}
