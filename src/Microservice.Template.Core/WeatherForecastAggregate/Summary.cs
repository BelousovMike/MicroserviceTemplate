namespace Microservice.Template.Core.WeatherForecastAggregate;

/// <summary>
/// Cводные данные о погоде.
/// </summary>
public class Summary : SmartEnum<Summary>
{
    /// <summary>
    /// Заморозки.
    /// </summary>
    public static readonly Summary Freezing = new(nameof(Freezing), 0);

    /// <summary>
    /// Прохладно.
    /// </summary>
    public static readonly Summary Bracing = new(nameof(Bracing), 1);

    /// <summary>
    /// Свежо.
    /// </summary>
    public static readonly Summary Chilly = new(nameof(Chilly), 2);

    /// <summary>
    /// Холодно.
    /// </summary>
    public static readonly Summary Cool = new(nameof(Cool), 3);

    /// <summary>
    /// Тепло.
    /// </summary>
    public static readonly Summary Mild = new(nameof(Mild), 4);

    /// <summary>
    /// Очень тепло.
    /// </summary>
    public static readonly Summary Warm = new(nameof(Warm), 5);

    /// <summary>
    /// Комфортная.
    /// </summary>
    public static readonly Summary Balmy = new(nameof(Balmy), 6);

    /// <summary>
    /// Жарко.
    /// </summary>
    public static readonly Summary Hot = new(nameof(Hot), 7);

    /// <summary>
    /// Душно.
    /// </summary>
    public static readonly Summary Sweltering = new(nameof(Sweltering), 8);

    /// <summary>
    /// Пекло.
    /// </summary>
    public static readonly Summary Scorching = new(nameof(Scorching), 9);

    private Summary(string name, int value)
        : base(name, value)
        {
    }
}
