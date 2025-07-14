namespace Microservice.Template.Core.WeatherForecastAggregate;

public class WeatherForecast
{
  public DateOnly Date { get; set; }

  public int TemperatureC { get; set; }

  public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

  public Summary? Summary { get; set; }
}
