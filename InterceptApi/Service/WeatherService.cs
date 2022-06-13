namespace InterceptApi.Service;

public class WeatherService : IWeatherService
{
    private readonly ILogger<WeatherService> _logger;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public WeatherService(ILogger<WeatherService> logger)
    {
        _logger = logger;
    }

    public IEnumerable<WeatherForecast> Get_SAVE_Temperature(int z1, int z2)
    {
        List<WeatherForecast> saveTemperature = new List<WeatherForecast>();
        try
        {
            saveTemperature = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = z1 / z2,
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToList();
        }
        catch (DivideByZeroException divZero)
        {
            string divisionByZeroInTheSaveMethodCatched = "division by zero in the save method catched";
            _logger.LogError(divisionByZeroInTheSaveMethodCatched);
            throw new MyKbcException(divisionByZeroInTheSaveMethodCatched, divZero, "mykbc/call_SAVE_TEMP_Zero_Div");

        }
        catch (Exception e)
        {
            string upsyError = "unexpected error";
            _logger.LogError(upsyError);
            throw new MyKbcException(upsyError, e, "mykbc/call_SAVE_TEMP_DunnoWhatHappened");
        }
        return saveTemperature;
    }
    
    public IEnumerable<WeatherForecast> GetTemperature(int z1, int z2)
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = z1/z2, 
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}