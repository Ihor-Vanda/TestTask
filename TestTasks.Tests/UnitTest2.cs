using TestTasks.WeatherFromAPI;

namespace TestTasks.Tests;

public class UnitTest2
{
    private WeatherManager weatherManager = new(new HttpClient());

    [Fact]
    public async Task GetCityCoordinatesAsync_ShouldReturnCoordinatesForValidCity()
    {
        var cityName = "Kyiv";
        var coordinates = await weatherManager.GetCityCoordinatesAsync(cityName);

        Assert.NotNull(coordinates);
        Assert.Equal(cityName, coordinates.CityName);
        Assert.True(coordinates.Latitude == 50.4500336);
        Assert.True(coordinates.Longitude == 30.5241361);
    }
}
