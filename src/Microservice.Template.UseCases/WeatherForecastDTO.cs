namespace Microservice.Template.UseCases;

public record WeatherForecastDto(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);
