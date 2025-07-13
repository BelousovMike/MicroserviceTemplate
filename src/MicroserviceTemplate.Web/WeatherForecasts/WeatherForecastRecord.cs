namespace MicroserviceTemplate.Web.WeatherForecasts;

public record WeatherForecastRecord(DateOnly Date, int TemperatureC, string? Summary, int TemperatureF);
