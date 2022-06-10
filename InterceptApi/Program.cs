using Castle.DynamicProxy;
using InterceptApi;
using InterceptApi.Controllers;
using InterceptApi.Interceptors;
using InterceptApi.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(new ProxyGenerator());
builder.Services.AddScoped<IInterceptor, ExceptionInterceptor>();
builder.Services.AddScoped<IInterceptor, LoggingInterceptor>();
builder.Services.AddProxiedScoped<IWeatherService, WeatherService>();

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
    
    public static void AddIngterceptedSingleton<TInterface, TImplementation, TInterceptor>
        (this IServiceCollection services)
        where TInterface : class
        where TImplementation : class, TInterface
        where TInterceptor : class, IInterceptor
    {
        services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();
        services.AddSingleton<TImplementation>();
        services.TryAddTransient<TInterceptor>();
        
        services.AddSingleton(provider =>
        {
            var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
            var impl = provider.GetRequiredService<TImplementation>();
            var interceptor = provider.GetRequiredService<IInterceptor>();
            return proxyGenerator.CreateInterfaceProxyWithTarget<TInterface>(impl, interceptor);
        });
    }
}

