using TestTasks.WeatherFromAPI;

namespace TestTasks.Tests;

public class UnitTest3
{
    private WeatherManager weatherManager = new(new HttpClient());

    [Fact]
    public async Task CompareWeather_ThrowsArgumentExceptionForInvalidCity()
    {
        // Arrange
        var invalidCity = "";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () => await weatherManager.CompareWeather(invalidCity, "Kyiv", 3));
    }
}
