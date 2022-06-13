using Microsoft.AspNetCore.Mvc.Filters;

namespace InterceptApi.Filters;

public class MyControllerLoggingFilter : IActionFilter
{
    private readonly ILogger<MyControllerLoggingFilter> _logger;

    /// <summary>
    /// DI for ILogger works here because of ServiceFilter attribute
    /// </summary>
    /// <param name="logger"></param>
    public MyControllerLoggingFilter(ILogger<MyControllerLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation($"BEFORE : {context.ActionDescriptor.DisplayName}");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation($"AFTER : {context.ActionDescriptor.DisplayName}");
    }
}