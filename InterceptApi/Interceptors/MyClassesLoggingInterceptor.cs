using Castle.DynamicProxy;

namespace InterceptApi.Interceptors;

public class MyClassesLoggingInterceptor : IInterceptor
{
    private readonly ILogger<MyClassesLoggingInterceptor> _logger;

    public MyClassesLoggingInterceptor(ILogger<MyClassesLoggingInterceptor> logger)
    {
        _logger = logger;
    }

    public void Intercept(IInvocation invocation)
    {
        string target = $"{invocation.TargetType.Namespace}.{invocation.TargetType.Name}.{invocation.Method.Name}";
        _logger.LogInformation($"BEFORE: {target}");
        try
        {
            invocation.Proceed();
        }
        finally
        {
            _logger.LogInformation($"AFTER: {target}");
        }
    }
}