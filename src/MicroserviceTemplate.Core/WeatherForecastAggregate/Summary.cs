namespace MicroserviceTemplate.Core.WeatherForecastAggregate;

public class Summary : SmartEnum<Summary>
{
  public static readonly Summary Freezing = new(nameof(Freezing), 0);
  public static readonly Summary Bracing = new(nameof(Bracing), 1);
  public static readonly Summary Chilly = new(nameof(Chilly), 2);
  public static readonly Summary Cool = new(nameof(Cool), 3);
  public static readonly Summary Mild = new(nameof(Mild), 4);
  public static readonly Summary Warm = new(nameof(Warm), 5);
  public static readonly Summary Balmy = new(nameof(Balmy), 6);
  public static readonly Summary Hot = new(nameof(Hot), 7);
  public static readonly Summary Sweltering = new(nameof(Sweltering), 8);
  public static readonly Summary Scorching = new(nameof(Scorching), 9);

  private Summary(string name, int value) : base(name, value)
  {
  }
}

