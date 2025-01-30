using System.Net;
using Moq;
using Moq.Protected;
using TestTasks.WeatherFromAPI;
using TestTasks.WeatherFromAPI.Models;

namespace TestTasks.Tests;

public class UnitTest5
{
    private readonly Mock<HttpMessageHandler> _mockHandler;
    private readonly HttpClient _httpClient;
    private readonly WeatherManager _weatherManager;

    public UnitTest5()
    {
        _mockHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHandler.Object);
        _weatherManager = new WeatherManager(_httpClient);
    }

    [Fact]
    public async Task GetWeatherDataAsync_ShouldReturnCityDataWithWeatherInfo()
    {
        // Arrange
        var cityData = new CityData
        {
            CityName = "Kyiv",
            Latitude = 50.4501,
            Longitude = 30.5236
        };

        var jsonResponse = "{\"list\": [{\"dt\": 1641579600, \"rain\": {\"3h\": 0.5}, \"main\": {\"temp\": 5.0}}]}";

        _mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse),
            });

        // Act
        var result = await _weatherManager.GetWeatherDataAsync(cityData, 3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cityData.CityName, result.CityName);
        Assert.Single(result.DailyWeather);
        Assert.Equal(5.0, result.DailyWeather[0].HourlyTemperatures[0].Temperature);
        Assert.Equal(0.5, result.DailyWeather[0].Rain);
    }
}
