using Castle.DynamicProxy;

namespace InterceptApi.Interceptors;

public class LoggingInterceptor : IInterceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        _logger = logger;
    }
    public void Intercept(IInvocation invocation)
    {
        //_logger.LogDebug($"Calling method {invocation.TargetType}.{invocation.Method.Name}.");
        
        _logger.LogInformation("LoggingInterceptor: Before target call");
        invocation.Proceed();
        _logger.LogInformation("LoggingInterceptor: After target call");
    }
}