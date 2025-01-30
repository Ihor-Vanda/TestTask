using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTasks.WeatherFromAPI.Models
{
    public class WeatherData
    {
        public double CurrentTemperature { get; set; }
        public double FeelsLikeTemperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string WeatherDescription { get; set; }

        public List<DailyWeatherData> DailyWeather { get; set; } = new();
        public List<HourlyWeatherData> HourlyWeather { get; set; } = new();
    }

    public class DailyWeatherData
    {
        public DateTime Date { get; set; }
        public double DayTemperature { get; set; }
        public double NightTemperature { get; set; }
        public double RainVolume { get; set; }
        public string Description { get; set; }
    }

    public class HourlyWeatherData
    {
        public DateTime DateTime { get; set; }
        public double Temperature { get; set; }
        public double RainVolume { get; set; }
    }
}