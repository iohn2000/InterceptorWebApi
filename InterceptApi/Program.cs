using Castle.DynamicProxy;
using InterceptApi.Filters;
using InterceptApi.Interceptors;
using InterceptApi.Service;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// add filter for dependency injection
builder.Services.AddScoped<MyControllerLoggingFilter>();

// castle core proxy generator needed to create proxy (interceptor) classes at runtime
builder.Services.AddSingleton(new ProxyGenerator());

//builder.Services.AddScoped<IExceptionFilter, MyExceptionFilter>();
//builder.Services.AddScoped<IInterceptor, ExceptionInterceptor>();

builder.Services.AddScoped<IInterceptor, MyClassesLoggingInterceptor>();
builder.Services.AddScoped<IActionFilter, MyControllerLoggingFilter>();

//
// add all proxy (interceptor) classes of type IInterceptor to WeatherService
//
//builder.Services.AddProxiedScoped<IWeatherService, WeatherService>();

//
// add a single specific IInterceptor to WeatherService
//
builder.Services.AddInterceptedScoped<IWeatherService, WeatherService, MyClassesLoggingInterceptor>();

// Add services to the container.
builder.Services.AddControllers(c =>
{
    c.Filters.Add(typeof(MyExceptionFilter)); // exception filter here is added to all controllers and all actions
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public static class ServicesExtensions
{
    public static void AddProxiedScoped<TInterface, TImplementation>(this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services.AddScoped<TImplementation>();
        services.AddScoped(typeof(TInterface), serviceProvider =>
        {
            var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
            var actual = serviceProvider.GetRequiredService<TImplementation>();
            var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();
            return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
        });
    }
    
    public static void AddInterceptedScoped<TInterface, TImplementation, TInterceptor>
        (this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IInterceptor
    {
        //services.TryAddScoped<IProxyGenerator, ProxyGenerator>();
        services.AddScoped<TImplementation>();
        services.TryAddTransient<TInterceptor>();
        
        services.AddScoped(typeof(TInterface), provider =>
        {
            var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
            var impl = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<IInterceptor>();
            return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(impl, interceptor);
        });
    }
}

