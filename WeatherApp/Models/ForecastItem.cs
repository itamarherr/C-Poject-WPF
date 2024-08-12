using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json;


namespace WeatherApp.Models
{
    public class ForecastItem
    {
        public string FormattedDate { get; set; }
        public DateTime DateTime { get; set; }


        public string Temperature { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }

    
    }
}
