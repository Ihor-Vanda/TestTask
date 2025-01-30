using TestTasks.WeatherFromAPI;

namespace TestTasks.Tests;

public class UnitTest4
{
    private WeatherManager weatherManager = new(new HttpClient());

    [Fact]
    public async Task CompareWeather_ThrowsArgumentExceptionForInvalidDayCount()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await weatherManager.CompareWeather("Kyiv", "Lviv", 6));
    }
}
