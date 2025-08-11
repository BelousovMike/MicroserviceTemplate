namespace Microservice.Template.UseCases;

/// <summary>
/// Прогноз погоды.
/// </summary>
/// <param name="Date">Дата.</param>
/// <param name="TemperatureC">Температура в градусах по Цельсию.</param>
/// <param name="TemperatureF">Темпеература в градусах по Фаренгейту.</param>
/// <param name="Summary">Описание.</param>
public record WeatherForecastDto(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);
