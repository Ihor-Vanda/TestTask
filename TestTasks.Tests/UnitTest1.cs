using TestTasks.WeatherFromAPI;
using TestTasks.WeatherFromAPI.Models;

namespace TestTasks.Tests;

public class UnitTest1
{
    private WeatherManager weatherManager = new(new HttpClient());
    [Fact]
    public async Task CompareWeather_ShouldReturnCorrectComparisonResult()
    {
        // Arrange
        var cityA = "London";
        var cityB = "Paris";
        int dayCount = 3;

        // Mock weather data for city A
        var cityAData = new CityData
        {
            CityName = cityA,
            DailyWeather = new List<DailyWeather>
            {
                new DailyWeather { Date = DateTime.Today, Rain = 0.5, HourlyTemperatures = new List<HourlyWeather> { new HourlyWeather { Temperature = 10.5 } } },
                new DailyWeather { Date = DateTime.Today.AddDays(1), Rain = 0.0, HourlyTemperatures = new List<HourlyWeather> { new HourlyWeather { Temperature = 12.0 } } },
                new DailyWeather { Date = DateTime.Today.AddDays(2), Rain = 1.5, HourlyTemperatures = new List<HourlyWeather> { new HourlyWeather { Temperature = 14.0 } } }
            }
        };

        // Mock weather data for city B
        var cityBData = new CityData
        {
            CityName = cityB,
            DailyWeather = new List<DailyWeather>
            {
                new DailyWeather { Date = DateTime.Today, Rain = 0.2, HourlyTemperatures = new List<HourlyWeather> { new HourlyWeather { Temperature = 11.0 } } },
                new DailyWeather { Date = DateTime.Today.AddDays(1), Rain = 0.0, HourlyTemperatures = new List<HourlyWeather> { new HourlyWeather { Temperature = 10.0 } } },
                new DailyWeather { Date = DateTime.Today.AddDays(2), Rain = 0.0, HourlyTemperatures = new List<HourlyWeather> { new HourlyWeather { Temperature = 13.0 } } }
            }
        };

        // Act
        var result = await weatherManager.CompareWeather(cityA, cityB, dayCount);

        // Assert
        Assert.Equal(2, result.WarmerDaysCount);  // London warmer on 2 days
        Assert.Equal(1, result.RainierDaysCount); // London rainier on 1 day
    }
}
