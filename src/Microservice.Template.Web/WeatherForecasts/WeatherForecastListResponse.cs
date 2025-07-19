namespace Microservice.Template.Web.WeatherForecasts;

/// <summary>
/// DTO для передачи списка прогнозов погоды в HTTP ответе.
/// </summary>
internal sealed class WeatherForecastListResponse
{
    /// <summary>
    /// Список прогнозов погоды.
    /// </summary>
    public List<WeatherForecastRecord> Weathers { get; set; } = [];
}
