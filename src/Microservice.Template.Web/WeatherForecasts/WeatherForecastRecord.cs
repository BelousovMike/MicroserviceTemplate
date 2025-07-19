namespace Microservice.Template.Web.WeatherForecasts;

/// <summary>
/// DTO для передачи данных о прогнозе погоды в API ответе.
/// </summary>
/// <param name="Date">Дата прогноза.</param>
/// <param name="TemperatureC">Температура в градусах Цельсия.</param>
/// <param name="Summary">Описание погоды (может быть null).</param>
/// <param name="TemperatureF">Температура в градусах Фаренгейта.</param>
internal sealed record WeatherForecastRecord(DateOnly Date, int TemperatureC, string? Summary, int TemperatureF);
