using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    class CurrentWeatherjsonModel
    {
        public MainInfo Main { get; set; }
        public WindInfo Wind { get; set; }
        [JsonProperty("clouds")]
        public CloudInfo Clouds { get; set; }
        public WeatherInfo[] Weather { get; set; }
    }
}
