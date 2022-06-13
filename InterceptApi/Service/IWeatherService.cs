namespace InterceptApi.Service;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> Get_SAVE_Temperature(int z1, int z2);
    
    IEnumerable<WeatherForecast> GetTemperature(int z1, int z2);
}