using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InterceptApi.Filters;

public class MyExceptionFilter : ExceptionFilterAttribute //, IExceptionFilter
{
    private readonly ILogger<MyControllerLoggingFilter> _logger;

    public MyExceptionFilter(ILogger<MyControllerLoggingFilter> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        _logger.LogError(
            $"Following Exception thrown:{context.Exception.Message} ({context.Exception.GetType().ToString()})");
        
        MyKbcException myKbcException = (MyKbcException)context.Exception;
        
        context.Result = new ObjectResult(new ProblemDetails
        {
            Title = context.Exception.Message,
            Status = 418,
            Type = context.Exception.GetType().ToString(),
            Instance = myKbcException.Uri,
            Detail = myKbcException.InnerException?.ToString(),
        });
    }
}