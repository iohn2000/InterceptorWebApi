using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InterceptApi.Filters;

public class MyExceptionFilter : IExceptionFilter
{
    private readonly ILogger<MyControllerLoggingFilter> _logger;

    public MyExceptionFilter(ILogger<MyControllerLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(
            $"Following Exception thrown:{context.Exception.Message} ({context.Exception.GetType().ToString()})");
        context.Result = new NotFoundResult();
    }
}