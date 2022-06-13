﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InterceptApi.Filters;

public class MyExceptionFilter : ExceptionFilterAttribute //, IExceptionFilter
{
    private readonly ILogger<MyExceptionFilter> _logger;

    public MyExceptionFilter(ILogger<MyExceptionFilter> logger)
    {
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        _logger.LogError(
            $"Following Exception thrown:{context.Exception.Message} ({context.Exception.GetType().ToString()})");
        
        MyKbcException myKbcException = context.Exception as MyKbcException;
        if (myKbcException != null)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = context.Exception.Message,
                Status = 418,
                Type = context.Exception.GetType().ToString(),
                Instance = myKbcException.Uri,
                Detail = myKbcException.InnerException?.ToString(),
            });    
        }
        else
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = context.Exception.Message,
                Status = 418,
                Type = context.Exception.GetType().ToString(),
                Instance = "mykbc/unknown",
                Detail = context.Exception.ToString(),
            });    
        }
        
    }
}