using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TestTasks.WeatherFromAPI.Models;

namespace TestTasks.WeatherFromAPI
{
    public class WeatherManager
    {
        private readonly HttpClient client;
        private readonly string ApiKey = File.ReadAllText("./WeatherFromAPI/apikey.txt");

        public WeatherManager(HttpClient _client)
        {
            client = _client ?? throw new ArgumentNullException(nameof(client));
        }

        //currently one call api needs subsribe to access free plan have only Current weather API, 3-hour forecast for 5 days API, 
        //Weather Maps - Current weather, 5 weather layers ,Air Pollution API, Geocoding API
        //so for test task was using 3-hour forecast for 5 days API that have similar data format

        public async Task<WeatherComparisonResult> CompareWeather(string cityA, string cityB, int dayCount)
        {
            if (string.IsNullOrWhiteSpace(cityA) || string.IsNullOrWhiteSpace(cityB))
                throw new ArgumentException("City names must be provided.");

            if (dayCount <= 0 || dayCount > 5)
                throw new ArgumentException("Day count must be between 1 and 5.");

            var cityAData = await GetCityCoordinatesAsync(cityA) ?? throw new ArgumentNullException($"Can't get location of {cityA}");
            var cityBData = await GetCityCoordinatesAsync(cityB) ?? throw new ArgumentNullException($"Can't get location of {cityB}");

            // Console.WriteLine(cityAObj.CityName + " " + cityAObj.Longitude + " " + cityAObj.Latitude);
            // Console.WriteLine(cityBObj.CityName + " " + cityBObj.Longitude + " " + cityBObj.Latitude);

            cityAData = await GetWeatherDataAsync(cityAData, dayCount);
            // foreach (var day in cityAData.DailyWeather)
            // {
            //     DisplayWeather(day);
            // }
            cityBData = await GetWeatherDataAsync(cityBData, dayCount);
            // foreach (var day in cityBData.DailyWeather)
            // {
            //     DisplayWeather(day);
            // }

            int warmerDaysCount = 0;
            int rainierDaysCount = 0;

            for (int i = 0; i < dayCount; i++)
            {
                var cityADailyWeather = cityAData.DailyWeather[i];
                var cityBDailyWeather = cityBData.DailyWeather[i];

                double avgTempCityA = cityADailyWeather.HourlyTemperatures.Average(temp => temp.Temperature);
                double avgTempCityB = cityBDailyWeather.HourlyTemperatures.Average(temp => temp.Temperature);

                if (avgTempCityA > avgTempCityB) warmerDaysCount++;

                if (cityADailyWeather.Rain > cityBDailyWeather.Rain) rainierDaysCount++;
            }

            return new WeatherComparisonResult(cityA, cityB, warmerDaysCount, rainierDaysCount);
        }

        public void DisplayWeather(DailyWeather dailyWeather)
        {
            if (dailyWeather == null)
            {
                Console.WriteLine("No weather data available.");
                return;
            }

            Console.WriteLine($"Date: {dailyWeather.Date.ToShortDateString()}");
            Console.WriteLine($"Rainfall: {dailyWeather.Rain} mm");

            if (dailyWeather.HourlyTemperatures == null || dailyWeather.HourlyTemperatures.Count == 0)
            {
                Console.WriteLine("No hourly weather data available.");
                return;
            }

            Console.WriteLine("Hourly temperatures:");
            foreach (var hourly in dailyWeather.HourlyTemperatures)
            {
                Console.WriteLine($"Time: {hourly.Time.ToShortTimeString()}, Temperature: {hourly.Temperature}°C");
            }
        }


        public async Task<CityData> GetCityCoordinatesAsync(string cityName)
        {
            string url = $"https://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={ApiKey}";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var locations = JArray.Parse(json);

                if (locations.Count == 0) return null;

                var location = locations[0];
                var cityCoordinates = new CityData
                {
                    CityName = location["name"]?.ToString(),
                    Latitude = (double)location["lat"],
                    Longitude = (double)location["lon"]
                };

                return cityCoordinates;
            }
            else
            {
                return null;
            }
        }

        public async Task<CityData> GetWeatherDataAsync(CityData cityData, int dayCount)
        {
            if (cityData == null)
                throw new ArgumentNullException("CityData cannot be null");

            string apiUrl = $"https://api.openweathermap.org/data/2.5/forecast?lat={cityData.Latitude}&lon={cityData.Longitude}&appid={ApiKey}&units=metric";

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.StatusCode == (System.Net.HttpStatusCode)429)
            {
                throw new Exception("Too many requests to the API. Please wait and try again later.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching weather data: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var weatherData = JsonSerializer.Deserialize<JsonElement>(json);

            var dailyWeather = weatherData.GetProperty("list")
                .EnumerateArray()
                .GroupBy(x => UnixTimeStampToDateTime(x.GetProperty("dt").GetInt64()).Date)
                .Select(group => new DailyWeather
                {
                    Date = group.Key,
                    Rain = group.Sum(x => x.TryGetProperty("rain", out var rain) ? rain.GetProperty("3h").GetDouble() : 0.0),
                    HourlyTemperatures = group.Select(x => new HourlyWeather
                    {
                        Time = UnixTimeStampToDateTime(x.GetProperty("dt").GetInt64()),
                        Temperature = x.GetProperty("main").GetProperty("temp").GetDouble()
                    }).ToList()
                })
                .ToList();

            cityData.DailyWeather = dailyWeather;

            return cityData;
        }


        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
        }

    }
}

