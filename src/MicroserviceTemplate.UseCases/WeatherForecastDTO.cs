namespace MicroserviceTemplate.UseCases;

public record WeatherForecastDTO(DateOnly Date, int TemperatureC, int TemperatureF, string? Summary);
