using InterceptApi.Filters;
using InterceptApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace InterceptApi.Controllers;

[ServiceFilter(typeof(MyControllerLoggingFilter))] // filter here is added on a per controller (for all actions in controller) basis as Attribute, also works before single actions!
[ApiController]
[Route("[controller]")]
public class MvcPipeLineController : ControllerBase
{
    private readonly IWeatherService _theWeather;
    private readonly ILogger<WeatherForecastController> _logger;

    public MvcPipeLineController(IWeatherService theWeather, ILogger<WeatherForecastController> logger)
    {
        _theWeather = theWeather;
        _logger = logger;
    }

    [HttpGet]
    [Route("tantrum")]
    public IActionResult ThrowTantrumInControllerAction()
    {
        int zero = 0;
        int upsy = 1 / zero;
        return Ok();
    }

    /// <summary>
    /// call a service class and this class throws unhandled exception
    /// </summary>
    /// <param name="z1"></param>
    /// <param name="z2"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("save")]
    public IEnumerable<WeatherForecast> GetSAVE([FromQuery]int z1, [FromQuery]int z2)
    {
        var theWeather =_theWeather.GetTemperature(z1,z2);
        return theWeather;
    }
    
    /// <summary>
    /// call a service class and this class throws unhandled exception
    /// </summary>
    /// <param name="z1"></param>
    /// <param name="z2"></param>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<WeatherForecast> Get([FromQuery]int z1, [FromQuery]int z2)
    {
        var theWeather =_theWeather.GetTemperature(z1,z2);
        return theWeather;
    }
}