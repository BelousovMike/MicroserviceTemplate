namespace Microservice.Template.Core.WeatherForecastAggregate;

/// <summary>
/// Прогноз погоды.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Дата.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Температура по Цельсию.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Температура по Фаренгейту.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    ///  Cводные данные о погоде.
    /// </summary>
    public Summary? Summary { get; set; }
}
