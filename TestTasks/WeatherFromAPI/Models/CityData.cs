using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTasks.WeatherFromAPI.Models
{
    public class CityData
    {
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<DailyWeather> DailyWeather { get; set; }
    }
}