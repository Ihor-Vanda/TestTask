using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTasks.WeatherFromAPI.Models
{
    public class HourlyWeather
    {
        public DateTime Time { get; set; }
        public double Temperature { get; set; }
    }
}