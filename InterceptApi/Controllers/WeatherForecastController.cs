using InterceptApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace InterceptApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    private readonly IWeatherService _theWeather;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService theWeather)
    {
        _logger = logger;
        _theWeather = theWeather;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get([FromQuery]int z1, [FromQuery]int z2)
    {
        _logger.LogDebug("in controller get()");
        return _theWeather.GetTemperature(z1,z2);
    }
}