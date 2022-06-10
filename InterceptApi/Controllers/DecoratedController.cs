using InterceptApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InterceptApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DecoratedController : ControllerBase
{
    private readonly IWeatherService _theWeather;
    private readonly ILogger<WeatherForecastController> _logger;
    // GET
    public DecoratedController(IWeatherService theWeather, ILogger<WeatherForecastController> logger)
    {
        _theWeather = theWeather;
        _logger = logger;
    }

    [Audit]
    [HttpGet(Name = "GetDecoratedWeather")]
    public IEnumerable<WeatherForecast> Get([FromQuery]int z1, [FromQuery]int z2)
    {
        _logger.LogDebug("in controller get()");
        return _theWeather.Get(z1,z2);
    }
}

public class AuditAttribute : ActionFilterAttribute
{
    private readonly ILogger<AuditAttribute> _logger;

    public AuditAttribute()
    {
        
    }
    
    public AuditAttribute(ILogger<AuditAttribute> logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
        var controllerName = actionDescriptor.ControllerName;
        var actionName = actionDescriptor.ActionName;
        var parameters = actionDescriptor.Parameters;
        var fullName = actionDescriptor.DisplayName;
        //_logger.LogDebug("{ControllerName}",controllerName);
        Console.WriteLine($"ControllerName: {controllerName}");
    }
}