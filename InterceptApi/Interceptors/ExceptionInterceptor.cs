using Castle.DynamicProxy;

namespace InterceptApi.Interceptors;

public class ExceptionInterceptor : IInterceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }
    public void Intercept(IInvocation invocation)
    {
        _logger.LogInformation("ExceptionInterceptor: Before target call");
        try
        {
            invocation.Proceed();
        }
        catch(Exception ex)
        {
            string msg = $"{invocation.Method.Name} threw: {ex.Message} with : {invocation.Arguments[0]} / {invocation.Arguments[1]}";
            _logger.LogError(msg);
        }
        finally
        {
            _logger.LogInformation("ExceptionInterceptor: After target call");
        }
    }
}