using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTasks.WeatherFromAPI.Models
{
    public class DailyWeather
    {
        public DateTime Date { get; set; }
        public double Rain { get; set; }
        public List<HourlyWeather> HourlyTemperatures { get; set; }
    }
}