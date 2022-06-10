namespace InterceptApi.Service;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> Get(int z1, int z2);
}